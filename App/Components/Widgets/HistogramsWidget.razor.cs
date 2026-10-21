namespace App.Components.Widgets;

using Core.Histogram;
using Core.History;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Pages.Shared;

public partial class HistogramsWidget : BaseWidget<WidgetPayload>
{

    public override bool HasContent() => HistogramSingleton.Entries.Count > 0;

    public override WidgetPayload InitializePayload()
    {
        WidgetPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        return payload;
    }
}
