using Core.Utility;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using ImageSharp = SixLabors.ImageSharp.Image;

namespace Core.Image.Processing;

public abstract class ImageProcessor<TSelf>
    where TSelf : ImageProcessor<TSelf>, new()
{
    protected ImageProcessor() { }

    public static TSelf Instance { get; } = new TSelf();
    public abstract byte[] ImageSignature { get; }
    public abstract ImageFormat HasCorrectSignature(ReadOnlySpan<byte> bytes);
    public abstract ImageDimensions GetDimensions(ReadOnlySpan<byte> bytes);

    public virtual ImageMetadata Encrypt(FileMetadata fileMetadata)
    {
        ImageFormat imageFormat = HasCorrectSignature(fileMetadata.FiledataBlob);
        if (imageFormat == ImageFormat.Unknown)
        {
            throw new ArgumentException("Invalid image signature.");
        }

        DecryptionKey key = new DecryptionKey(new double[10]);
        using var img = ImageSharp.Load<Rgba32>(fileMetadata.FiledataBlob);
        byte[] imgPixels = new byte[img.Width * img.Height * 4];
        byte[] encryptedPixels = new byte[imgPixels.Length];
        img.CopyPixelDataTo(imgPixels);
        img.CopyPixelDataTo(encryptedPixels);

        ImageCryptoBindings.CryptoStatus status = ImageCryptoBindings.Encrypt(
            encryptedPixels,
            new UIntPtr((ulong)encryptedPixels.Length),
            (uint)img.Width,
            (uint)img.Height,
            ImageCryptoBindings.CryptoPixelFormat.RGBA,
            key.Value
        );

        using var imgEncrypted = ImageSharp.LoadPixelData<Rgba32>(
            encryptedPixels,
            (int)img.Width,
            (int)img.Height
        );

        using var filedataEncryptedBlob = new MemoryStream();
        imgEncrypted.Save(filedataEncryptedBlob, new PngEncoder());

        return new ImageMetadata.ImageMetadataBuilder()
            .FromFileMetadata(fileMetadata)
            .WithImageFormat(imageFormat)
            .WithKey(key)
            .WithFiledataEncryptedBlob(filedataEncryptedBlob.ToArray())
            .WithDimensions(new ImageDimensions((uint)img.Width, (uint)img.Height))
            .WithHasAlpha()
            .Build();
    }

    public ImageMetadata Decrypt(FileMetadata fileMetadata, DecryptionKey key)
    {
        ImageFormat imageFormat = HasCorrectSignature(fileMetadata.FiledataEncryptedBlob);
        if (imageFormat == ImageFormat.Unknown)
        {
            throw new ArgumentException("Invalid image signature.");
        }

        using var img = ImageSharp.Load<Rgba32>(fileMetadata.FiledataEncryptedBlob);
        // file is encrypted so while decoding the file, we get the encrypted pixels
        byte[] encryptedPixels = new byte[img.Width * img.Height * 4];
        byte[] imgPixels = new byte[encryptedPixels.Length];
        img.CopyPixelDataTo(encryptedPixels);
        // we must copy those pixels, because the decrypted pixels will be loaded into the same array
        img.CopyPixelDataTo(imgPixels);

        if (key.Value.Length != 10)
        {
            throw new ArgumentException("Key must contain exactly 10 doubles.");
        }

        ImageCryptoBindings.CryptoStatus status = ImageCryptoBindings.Decrypt(
            imgPixels,
            new UIntPtr((ulong)imgPixels.Length),
            (uint)img.Width,
            (uint)img.Height,
            ImageCryptoBindings.CryptoPixelFormat.RGBA,
            key.Value
        );

        using var imgDecrypted = ImageSharp.LoadPixelData<Rgba32>(
            imgPixels,
            (int)img.Width,
            (int)img.Height
        );

        using var filedataBlob = new MemoryStream();
        imgDecrypted.Save(filedataBlob, new PngEncoder());

        return new ImageMetadata.ImageMetadataBuilder()
            .FromFileMetadata(fileMetadata)
            .WithFiledataBlob(filedataBlob.ToArray())
            .WithImageFormat(imageFormat)
            .WithKey(key)
            .WithDimensions(new ImageDimensions((uint)img.Width, (uint)img.Height))
            .WithHasAlpha()
            .Build();
    }
}
