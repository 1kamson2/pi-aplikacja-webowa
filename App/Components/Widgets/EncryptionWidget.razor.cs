namespace App.Components.Widgets;

using Core.Algorithm;
using Core.Database;
using Core.Histogram;
using Core.History;
using Core.Image.Processing;
using Core.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using Microsoft.JSInterop;
using Pages.Shared;

public sealed partial class EncryptionWidget : BaseWidget<WidgetPayload>
{
    public static byte[]? encryptedImagePixels;
    public static double[]? decryptionKey;
    public static uint width;
    public static uint height;

    public override bool HasContent() => DragAndDropWidget.FileMetadata?.FiledataBlob is not null;

    public override WidgetPayload InitializePayload()
    {
        WidgetPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        return payload;
    }

    public void StartEncryption()
    {
        if (!HasContent())
        {
            NotificationsService.Error("Brak obrazu do szyfrowania.", "Wybierz obraz, który chcesz zaszyfrować.");
            return;
        }
        FileMetadata fileMetadata = DragAndDropWidget.FileMetadata!;
        ImageMetadata encryptedImageMetadata = PngProcessor.Encrypt(fileMetadata);
        FileMetadata encryptedFiledata = new FileMetadata.FileMetadataBuilder()
            .FromImageMetadata(encryptedImageMetadata)
            .Build();

        bool successFiledata = DragAndDropWidget.ChangeFileMetadata(encryptedFiledata);
        bool successImageMetadata = DragAndDropWidget.ChangeImageMetadata(encryptedImageMetadata);

        if (!successFiledata)
        {
            NotificationsService.Error("Metadane pliku są niepoprawne.");
            return;
        }

        if (!successImageMetadata)
        {
            NotificationsService.Error("Metadane obrazu są niepoprawne.");
            return;
        }

        HistorySingleton.Add(encryptedImageMetadata);
        HistogramSingleton.Add(encryptedImageMetadata);
    }
}
