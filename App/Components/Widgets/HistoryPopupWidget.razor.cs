using Microsoft.AspNetCore.Components;

namespace App.Components.Widgets;

using Core.History;
using Pages.Shared;

public partial class HistoryPopupWidget : Widget
{
    [Parameter]
    public EventCallback<ButtonPayload> ButtonPushedEvent { get; set; }
    public HistorySingleton HistorySingleton = HistorySingleton.Instance;

    public override bool HasContent() => HistorySingleton.Entries.Count > 0;

    protected virtual async Task RelayButtonPayload(ButtonPayload e)
    {
        if (ButtonPushedEvent.HasDelegate)
        {
            await ButtonPushedEvent.InvokeAsync(e);
        }
    }
}
