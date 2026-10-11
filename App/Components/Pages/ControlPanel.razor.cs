using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace App.Components.Pages;

using Shared;
using static Shared.WidgetType;
using static Shared.WidgetAction;

public partial class ControlPanel : BasePage
{
    private bool _isPopupOpen = false;
    private WidgetType? _widgetType = null;

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
                _widgetType = e.Type;
                break;
            case WidgetAction.ClosePopup:
                _isPopupOpen = false;
                _widgetType = null;
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
