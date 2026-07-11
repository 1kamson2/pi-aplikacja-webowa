using Core.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using static Core.Components.WidgetAction;
namespace App.Components.Pages;

public partial class ControlPanel : BasePage
{
    private bool _isPopupOpen = false;
    public ControlPanel()
    {
        Destination = "/";
    }

    public async Task OnWidgetClicked(WidgetPayload e)
    {
        switch (e.Action)
        {
            case OpenPopup:
                _isPopupOpen = true;
                break;
            case ClosePopup:
                _isPopupOpen = false;
                break;

        }
    }

    public void OnButtonClicked(ButtonPayload e)
    {
        switch (e.Action)
        {
            case WidgetAction.Accept or WidgetAction.ClosePopup:
                _isPopupOpen = false;
                break;
        }
    }

    public override void OnMouseScroll(WheelEventArgs e)
    {
        if (e.DeltaY < 0)
        {
            NavigationManager.NavigateTo(Destination);
        }
    }
}