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
    public override WidgetPayload InitializePayload()
    {
        WidgetPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        return payload;
    }

    public override bool HasContent() =>
        DragAndDropWidget.FileMetadata?.FiledataEncryptedBlob is not null
        && DragAndDropWidget.ImageMetadata?.FiledataEncryptedBlob is not null;

    public void StartDecryption()
    {
        if (!HasContent())
        {
            NotificationsService.Error("Brak obrazu do odszyfrowania.");
            return;
        }

        if (AlgorithmSingleton.SelectedAlgorithm is null)
        {
            NotificationsService.Error("Nie wybrano algortymu.");
            return;
        }

        ImageMetadata imageMetadata = ImageProcessor.Decrypt(
            DragAndDropWidget.FileMetadata,
            DragAndDropWidget.ImageMetadata.Key.Value!,
            AlgorithmSingleton.SelectedAlgorithm.Id!
        );
        bool successFiledata = DragAndDropWidget.ChangeFileMetadata(
            new FileMetadata.FileMetadataBuilder().FromImageMetadata(imageMetadata).Build()
        );
        bool successImageMetadata = DragAndDropWidget.ChangeImageMetadata(imageMetadata);

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
        return;
    }
}
