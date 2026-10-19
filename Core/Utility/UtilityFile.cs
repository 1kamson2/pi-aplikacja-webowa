using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace Core.Utility;

public enum ImageFormat
{
    Unknown,
    Jpg,
    Png,
}

public readonly record struct ImageDimensions(uint Width, uint Height);

public readonly record struct DecryptionKey(double[] Value);

public record FileMetadata(
    ImageDimensions Dimensions,
    byte[] FileData,
    byte[] Pixels,
    bool HasAlpha
);

public record ImageMetadata(
    ImageFormat ImageFormat,
    DecryptionKey? Key,
    ImageDimensions Dimensions,
    byte[] FileData,
    byte[] Pixels,
    bool HasAlpha
) : FileMetadata(Dimensions, FileData, Pixels, HasAlpha);

public static class UtilityFile
{
    public enum SizeUnits : uint
    {
        Byte = 1,
        KiloByte = 1024,
        MegaByte = KiloByte * 1024,
    }

    public static float Convert(SizeUnits from, SizeUnits to, float size)
    {
        float bytes = size * (float)from;
        return bytes / (float)to;
    }

    public static FileMetadata DecodeFile(byte[] fileBytes)
    {
        using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(fileBytes);
        var pixels = new byte[image.Width * image.Height * 4];
        image.CopyPixelDataTo(pixels);
        return new FileMetadata(
            Dimensions: new ImageDimensions((uint)image.Width, (uint)image.Height),
            FileData: fileBytes,
            Pixels: pixels,
            HasAlpha: true
        );
    }

    public static FileMetadata EncodeFile(byte[] imagePixels, uint width, uint height)
    {
        using var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(
            imagePixels,
            (int)width,
            (int)height
        );
        using var stream = new MemoryStream();
        image.Save(stream, new PngEncoder());
        return new FileMetadata(
            Dimensions: new ImageDimensions(width, height),
            FileData: stream.ToArray(),
            Pixels: imagePixels,
            HasAlpha: true
        );
    }
}
