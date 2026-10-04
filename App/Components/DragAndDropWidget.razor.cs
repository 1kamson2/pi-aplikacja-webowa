using Microsoft.AspNetCore.Components.Forms;

namespace App.Components;

public partial class DragAndDropWidget : Widget
{
    private const float MaxFileSizeInMB = 5;
    private readonly float MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
    private static readonly byte[] PngSignature =
    {
        0x89,
        0x50,
        0x4E,
        0x47,
        0x0D,
        0x0A,
        0x1A,
        0x0A,
    };
    public string? FileLoadErrorMessage { get; private set; }
    public string? FileName { get; private set; }
    public byte[] ImageData { get; private set; } = Array.Empty<byte>();

    private bool hasError() => !string.IsNullOrEmpty(FileLoadErrorMessage);
    private bool hasImage() => ImageData.Length > 0;
    private bool isImageValid(string contentType) => string.Equals(contentType, "image/png", StringComparison.OrdinalIgnoreCase);
    private bool hasImageCorrectSignature(byte[] bytes) => !(bytes.Length < PngSignature.Length || !bytes.AsSpan(0, PngSignature.Length).SequenceEqual(PngSignature));
    private float ImageSizeInMB()
    {
        if (hasImage())
        {
            return (float) ImageData.Length / (1024 * 1024);
        }

        return float.MaxValue;
    }
    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        FileLoadErrorMessage = null;

        if (e.FileCount > 1)
        {
            FileLoadErrorMessage = "Brak wsparcia dla kilku plików";
            return;
        }

        if (!isImageValid(e.File.ContentType))
        {
            FileLoadErrorMessage = "Dozwolone są tylko pliki PNG.";
            return;
        }

        try
        {
            using var ms = new MemoryStream();
            await e.File.OpenReadStream((long) MaxFileSizeInBytes).CopyToAsync(ms);
            var bytes = ms.ToArray();

            if (!hasImageCorrectSignature(bytes))
            {
                FileLoadErrorMessage = "Plik nie jest poprawnym obrazem PNG.";
                return;
            }

            ImageData = bytes;
            FileName = e.File.Name;
        }
        catch (IOException)
        {
            FileLoadErrorMessage = $"Plik jest za duży (max {MaxFileSizeInMB} MB).";
        }
    }
}
