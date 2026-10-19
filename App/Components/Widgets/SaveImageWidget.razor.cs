namespace App.Components.Widgets;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Pages.Shared;

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

    public async Task OnClickSaveImage()
    {
        var imageData = DragAndDropWidget.FileData;

        using var ms = new MemoryStream(imageData);
        var result = await CommunityToolkit.Maui.Storage.FileSaver.Default.SaveAsync(
            "",
            ms,
            CancellationToken.None
        );

        //if (!result.IsSuccessful && result.Exception is not null)
        //    throw result.Exception;
    }
}
