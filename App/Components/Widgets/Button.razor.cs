namespace App.Components.Widgets;

using Pages.Shared;

public partial class Button : BaseButton<ButtonPayload>
{
    public override ButtonPayload InitializePayload()
    {
        ButtonPayload payload = new();
        payload.Type = InstanceType;
        payload.Action = ActionType;
        payload.Id = Id;
        payload.Content = Content;
        return payload;
    }
}
