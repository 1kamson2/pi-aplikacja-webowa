namespace App.Components.Widgets;

using Core.History;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Pages.Shared;

public partial class HistogramsWidget : BaseWidget<WidgetPayload>
{
    public override WidgetPayload InitializePayload()
    {
        WidgetPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        return payload;
    }
}
