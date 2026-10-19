namespace App.Components.Widgets;

using Core.History;
using Core.Utility;
using Core.Image.Processing;
using Microsoft.AspNetCore.Components;
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

    public void StartDecryption()
    {
        ImageMetadata imageMetadata = PngProcessor.Instance.Decrypt(
            DragAndDropWidget.FileData,
            EncryptionWidget.decryptionKey
        );
        FileMetadata fileEncoded = UtilityFile.EncodeFile(
            imageMetadata.Pixels,
            imageMetadata.Dimensions.Width,
            imageMetadata.Dimensions.Height
        );
        if (fileEncoded.FileData is not null)
        {
            EncryptionWidget._ShouldEncrypt = true;
            DragAndDropWidget.FileData = fileEncoded.FileData;
        }
        return;
    }
}
