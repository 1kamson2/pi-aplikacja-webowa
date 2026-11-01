using Microsoft.AspNetCore.Components;

namespace App.Components.Widgets;

using Core.Algorithm;
using Pages.Shared;

public partial class AlgorithmPopupWidget : Widget
{
    [Parameter]
    public EventCallback<ButtonPayload> ButtonPushedEvent { get; set; }
    public override bool HasContent() => AlgorithmSingleton.Entries.Count > 0;

    protected virtual async Task RelayButtonPayload(ButtonPayload e)
    {
        if (ButtonPushedEvent.HasDelegate)
        {
            await ButtonPushedEvent.InvokeAsync(e);
        }
    }
}
