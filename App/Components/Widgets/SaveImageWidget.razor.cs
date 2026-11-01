namespace App.Components.Widgets;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Pages.Shared;
using System.Diagnostics;

public partial class SaveImageWidget : BaseWidget<WidgetPayload>
{
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    public override WidgetPayload InitializePayload()
    {
        WidgetPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        return payload;
    }

    public override bool HasContent() =>
        DragAndDropWidget.FileMetadata?.FiledataEncryptedBlob is not null;

    public async Task OnClickSaveImage()
    {
        if (!HasContent())
        {
            NotificationsService.Error("Nie udało się zapisać obrazu.", "Obraz musi zostać wcześniej zaszyfrowany.");
            return;
        }

        // blob contains some data
        byte[] filedataEncryptedBlob = DragAndDropWidget.FileMetadata?.FiledataEncryptedBlob!;
        try
        {

            using var ms = new MemoryStream(filedataEncryptedBlob);
            var result = await CommunityToolkit.Maui.Storage.FileSaver.Default.SaveAsync(
                "zaszyfrowany_obraz",
                ms,
                CancellationToken.None
            );
            NotificationsService.Success("Udało się zapisać obraz!");
        } catch (Exception ex)
        {
            NotificationsService.Error("Błąd podczas zapisywania obrazu.");
            Debug.WriteLine(ex.Message);
        }
    }
}
