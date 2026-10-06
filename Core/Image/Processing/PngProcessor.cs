using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Image.Processing;

public class PngProcessor : IImageProcessor<PngProcessor>
{
    // Use it as static class
    private PngProcessor() { }

    public byte[] ImageSignature { get; } = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

    public static PngProcessor Instance { get; } = new PngProcessor();

    public bool IsImage(string contentType) =>
        string.Equals(contentType, "image/png", StringComparison.OrdinalIgnoreCase);

    public bool HasCorrectSignature(ReadOnlySpan<byte> bytes) => bytes.StartsWith(ImageSignature);

    public byte[] Process(byte[] imageData, string contentType)
    {
        if (!IsImage(contentType))
        {
            // TODO: Add normalized outputs.
            throw new ArgumentException("the data is not an image");
        }
        if (!HasCorrectSignature(imageData.AsSpan()))
        {
            throw new InvalidDataException("image has incorrect signature");
        }

        // Callback (no processing for now)
        return imageData;
    }
}
