using Microsoft.AspNetCore.Components;

namespace App.Components.Pages.Shared;

public enum WidgetType
{
    None = 0,
    Button,
    DragAndDrop,
    SaveImage,
    EncryptionHistory,
    EncryptedImage,
    Histograms,
    Algorithms,
}

public enum WidgetAction
{
    Accept = 0,
    Delete,
    Modify,
    OpenPopup,
    ClosePopup,
}

public interface IWidgetPayload
{
    public WidgetType Type { get; set; }
    public WidgetAction Action { get; set; }
}

public struct WidgetPayload : IWidgetPayload
{
    public WidgetType Type { get; set; }
    public WidgetAction Action { get; set; }
}

public abstract partial class BaseWidget<PayloadType> : ComponentBase
    where PayloadType : IWidgetPayload, new()
{
    [Parameter]
    public EventCallback<PayloadType> ActionRequestedEvent { get; init; }

    [Parameter]
    public required WidgetAction ActionType { get; init; }

    [Parameter]
    public required WidgetType InstanceType { get; init; }

    [Parameter]
    public RenderFragment? Content { get; init; }
    public abstract PayloadType InitializePayload();

    protected virtual async Task EmitSignal()
    {
        if (ActionRequestedEvent.HasDelegate)
        {
            await ActionRequestedEvent.InvokeAsync(InitializePayload());
        }
    }
}
