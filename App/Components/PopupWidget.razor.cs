using Microsoft.AspNetCore.Components;

namespace App.Components;

using Pages.Shared;

public partial class PopupWidget : Widget
{
    [Parameter]
    public EventCallback<ButtonPayload> ButtonPushedEvent { get; set; }

    protected virtual async Task RelayButtonPayload(ButtonPayload e)
    {
        if (ButtonPushedEvent.HasDelegate)
        {
            await ButtonPushedEvent.InvokeAsync(e);
        }
    }
}
