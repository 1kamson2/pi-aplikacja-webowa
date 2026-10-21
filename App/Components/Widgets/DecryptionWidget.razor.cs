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

public sealed partial class DecryptionWidget : BaseWidget<WidgetPayload>
{
    AlgorithmSingleton AlgorithmSingleton = AlgorithmSingleton.Instance;

    public override WidgetPayload InitializePayload()
    {
        WidgetPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        return payload;
    }

    public override bool HasContent() =>
        DragAndDropWidget.FileMetadata?.FiledataEncryptedBlob is not null;

    public void StartDecryption()
    {
        ImageMetadata imageMetadata = PngProcessor.Instance.Decrypt(
            DragAndDropWidget.FileMetadata,
            DragAndDropWidget.ImageMetadata.Key.Value
        );
        DragAndDropWidget.FileMetadata = new FileMetadata.FileMetadataBuilder()
            .FromImageMetadata(imageMetadata)
            .Build();
        DragAndDropWidget.ImageMetadata = imageMetadata;
        DragAndDropWidget.ShowEncryptedImage = false;
        return;
    }
}
