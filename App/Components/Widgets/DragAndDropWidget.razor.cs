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
    public static string? FileLoadErrorMessage { get; private set; }
    public static FileMetadata? FileMetadata { get; set; }
    public static ImageMetadata? ImageMetadata { get; set; }
    public static bool ShowEncryptedImage { get; set; } = false;

    private bool HasError() => !string.IsNullOrEmpty(FileLoadErrorMessage);

    public override bool HasContent() =>
        FileMetadata?.FiledataBlob?.Length > 0 || FileMetadata?.FiledataEncryptedBlob?.Length > 0;

    private byte[] DisplayImage() =>
        ShowEncryptedImage ? FileMetadata.FiledataEncryptedBlob : FileMetadata.FiledataBlob;

    private float ImageSizeInMB() =>
        UtilityFile.Convert(
            UtilityFile.SizeUnits.Byte,
            UtilityFile.SizeUnits.MegaByte,
            FileMetadata?.FiledataBlob?.Length ?? 0
        );

    private async Task OnFileDeleted(MouseEventArgs e)
    {
        FileMetadata = null;
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
            using MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream((long)MaxFileSizeInBytes).CopyToAsync(ms);
            FileMetadata = new FileMetadata.FileMetadataBuilder()
                .WithFilename(e.File.Name)
                .WithFiledataBlob(ms.ToArray())
                .Build();
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
