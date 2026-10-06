using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace App.Components.Pages;

using Shared;

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
            case WidgetAction.OpenPopup:
                _isPopupOpen = true;
                break;
            case WidgetAction.ClosePopup:
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

    public override void OnKeyboardKeyClicked(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "ArrowUp":
                NavigationManager.NavigateTo(Destination);
                break;
        }
    }
}
