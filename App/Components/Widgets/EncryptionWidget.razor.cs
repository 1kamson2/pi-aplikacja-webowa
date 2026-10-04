namespace App.Components.Widgets;

using Core.History;
using Core.Image.Processing;
using Core.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Pages.Shared;

public sealed partial class EncryptionWidget : BaseWidget<WidgetPayload>
{
    public static byte[]? encryptedImagePixels;
    public static double[]? decryptionKey;
    public static uint width;
    public static uint height;
    public static bool _ShouldEncrypt = true;

    public override WidgetPayload InitializePayload()
    {
        WidgetPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        return payload;
    }

    public void StartEncryption()
    {
        ImageMetadata imageMetadata = PngProcessor.Instance.Encrypt(
            DragAndDropWidget.FileData,
            true
        );
        FileMetadata fileEncoded = UtilityFile.EncodeFile(
            imageMetadata.Pixels,
            imageMetadata.Dimensions.Width,
            imageMetadata.Dimensions.Height
        );
        if (fileEncoded.FileData is not null)
        {
            EncryptionWidget.decryptionKey = imageMetadata.Key.Value.Value;
            EncryptionWidget.encryptedImagePixels = imageMetadata.Pixels;
            EncryptionWidget.width = imageMetadata.Dimensions.Width;
            EncryptionWidget.height = imageMetadata.Dimensions.Height;
            EncryptionWidget._ShouldEncrypt = false;
            DragAndDropWidget.FileData = fileEncoded.FileData;
        }
    }
}
