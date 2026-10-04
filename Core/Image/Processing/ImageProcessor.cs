using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Core.Utility;
using SixLabors.ImageSharp.Formats;

namespace Core.Image.Processing;

public abstract class ImageProcessor<TSelf>
    where TSelf : ImageProcessor<TSelf>, new()
{
    protected ImageProcessor() { }

    public static TSelf Instance { get; } = new TSelf();
    public abstract byte[] ImageSignature { get; }
    public abstract ImageFormat HasCorrectSignature(ReadOnlySpan<byte> bytes);
    public abstract ImageDimensions GetDimensions(ReadOnlySpan<byte> bytes);

    public virtual ImageMetadata Encrypt(byte[] fileData, bool hasAlpha = false)
    {
        ImageFormat imageFormat = HasCorrectSignature(fileData);
        if (imageFormat == ImageFormat.Unknown)
        {
            throw new ArgumentException("Invalid image signature.");
        }
        FileMetadata fileMetadata = UtilityFile.DecodeFile(fileData);
        double[] key = new double[10];

        ImageCryptoBindings.CryptoPixelFormat pixelFormat = hasAlpha
            ? ImageCryptoBindings.CryptoPixelFormat.RGBA
            : ImageCryptoBindings.CryptoPixelFormat.RGB;

        ImageCryptoBindings.CryptoStatus status = ImageCryptoBindings.Encrypt(
            fileMetadata.Pixels,
            new UIntPtr((ulong)fileMetadata.Pixels.Length),
            fileMetadata.Dimensions.Width,
            fileMetadata.Dimensions.Height,
            pixelFormat,
            key
        );
        return new ImageMetadata(
            ImageFormat: imageFormat,
            Key: new DecryptionKey(key),
            FileData: fileData,
            Pixels: fileMetadata.Pixels,
            Dimensions: fileMetadata.Dimensions,
            HasAlpha: hasAlpha
        );
    }

    public ImageMetadata Decrypt(byte[] fileData, double[] key, bool hasAlpha = false)
    {
        ImageFormat imageFormat = HasCorrectSignature(fileData);
        if (imageFormat == ImageFormat.Unknown)
        {
            throw new ArgumentException("Invalid image signature.");
        }
        FileMetadata fileMetadata = UtilityFile.DecodeFile(fileData);

        if (key is null || key.Length != 10)
        {
            throw new ArgumentException("Key must contain exactly 10 doubles.");
        }

        ImageCryptoBindings.CryptoPixelFormat format = ImageCryptoBindings.CryptoPixelFormat.RGBA;

        ImageCryptoBindings.CryptoStatus status = ImageCryptoBindings.Decrypt(
            fileMetadata.Pixels,
            new UIntPtr((ulong)fileMetadata.Pixels.Length),
            fileMetadata.Dimensions.Width,
            fileMetadata.Dimensions.Height,
            format,
            key
        );

        return new ImageMetadata(
            ImageFormat: imageFormat,
            Key: new DecryptionKey(key),
            FileData: fileData,
            Pixels: fileMetadata.Pixels,
            Dimensions: fileMetadata.Dimensions,
            HasAlpha: hasAlpha
        );
    }
}
