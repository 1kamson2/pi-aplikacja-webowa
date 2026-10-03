using Microsoft.AspNetCore.Components.Forms;

namespace App.Components;

public partial class DragAndDropWidget : Widget
{
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
    private static readonly byte[] PngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    private string? _error;
    public byte[]? ImageData { get; private set; }
    public string? FileName { get; private set; }

    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        _error = null;
        var file = e.File;

        if (!string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
        {
            _error = "Dozwolone są tylko pliki PNG.";
            return;
        }

        try
        {
            using var ms = new MemoryStream();
            await file.OpenReadStream(MaxFileSize).CopyToAsync(ms);
            var bytes = ms.ToArray();

            // dodatkowa weryfikacja – sprawdzenie sygnatury PNG
            if (bytes.Length < PngSignature.Length ||
                !bytes.AsSpan(0, PngSignature.Length).SequenceEqual(PngSignature))
            {
                _error = "Plik nie jest poprawnym obrazem PNG.";
                return;
            }

            ImageData = bytes;
            FileName = file.Name;
        }
        catch (IOException)
        {
            _error = $"Plik jest za duży (max {MaxFileSize / 1024 / 1024} MB).";
        }
    }
}
