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
    AlgorithmSingleton AlgorithmSingleton = AlgorithmSingleton.Instance;

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
        FileMetadata fileMetadata = DragAndDropWidget.FileMetadata;
        ImageMetadata encryptedImageMetadata = PngProcessor.Instance.Encrypt(fileMetadata);
        FileMetadata encryptedFiledata = new FileMetadata.FileMetadataBuilder()
            .FromImageMetadata(encryptedImageMetadata)
            .Build();
        DragAndDropWidget.FileMetadata = encryptedFiledata;
        DragAndDropWidget.ImageMetadata = encryptedImageMetadata;
        HistorySingleton.Instance.Add(encryptedImageMetadata);
        HistogramSingleton.Instance.Add(encryptedImageMetadata);
        DragAndDropWidget.ShowEncryptedImage = true;
    }
}
