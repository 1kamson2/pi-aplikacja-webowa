namespace App.Components;

using Pages.Shared;

public partial class Widget : BaseWidget<WidgetPayload>
{
    public override WidgetPayload InitializePayload()
    {
        WidgetPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        return payload;
    }
}
