namespace Core.Image.Processing;

using Core.Utility;

public sealed class PngProcessor : ImageProcessor<PngProcessor>
{
    // Use it as static class
    public PngProcessor() { }

    public override byte[] ImageSignature { get; } =
        { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

    public override ImageFormat HasCorrectSignature(ReadOnlySpan<byte> bytes) =>
        bytes.StartsWith(ImageSignature) ? ImageFormat.Png : ImageFormat.Unknown;

    public override ImageDimensions GetDimensions(ReadOnlySpan<byte> bytes) =>
        new ImageDimensions(
            (uint)((bytes[16] << 24) | (bytes[17] << 16) | (bytes[18] << 8) | bytes[19]),
            (uint)((bytes[20] << 24) | (bytes[21] << 16) | (bytes[22] << 8) | bytes[23])
        );
}
