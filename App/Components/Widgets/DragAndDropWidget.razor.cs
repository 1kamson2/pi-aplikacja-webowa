using Core.Image.Processing;
using Core.Utility;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace App.Components.Widgets;

public partial class DragAndDropWidget : Widget
{
    private const float MaxFileSizeInMB = 5;
    private readonly float MaxFileSizeInBytes = UtilityFile.Convert(
        UtilityFile.SizeUnits.MegaByte,
        UtilityFile.SizeUnits.Byte,
        MaxFileSizeInMB
    );
    public string? FileLoadErrorMessage { get; private set; }
    public string? FileName { get; private set; }
    public byte[] ImageData { get; private set; } = Array.Empty<byte>();

    private bool HasError() => !string.IsNullOrEmpty(FileLoadErrorMessage);

    private bool HasImage() => ImageData.Length > 0;

    private float ImageSizeInMB() =>
        UtilityFile.Convert(
            UtilityFile.SizeUnits.Byte,
            UtilityFile.SizeUnits.MegaByte,
            ImageData.Length
        );

    private async Task OnFileDeleted(MouseEventArgs e)
    {
        ImageData = Array.Empty<byte>();
        FileName = null;
        FileLoadErrorMessage = null;
        await Task.CompletedTask;
    }

    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        FileLoadErrorMessage = null;

        if (e.FileCount > 1)
        {
            FileLoadErrorMessage = "Brak wsparcia dla kilku plików";
            return;
        }

        try
        {
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                await e.File.OpenReadStream((long)MaxFileSizeInBytes).CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            ImageData = PngProcessor.Instance.Process(bytes, e.File.ContentType);
            FileName = e.File.Name;
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidDataException)
        {
            FileLoadErrorMessage = $"Błąd podczas wczytywania pliku: `{ex.Message}`";
        }
        catch (IOException ex)
        {
            FileLoadErrorMessage = $"Ten plik jest zbyt duży. {ex.Message}";
        }
    }
}
