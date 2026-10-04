using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace App.Components.Pages;

using Core.Image.Processing;
using global::App.Components.Widgets;
using Shared;
using static Shared.WidgetAction;
using static Shared.WidgetType;

public partial class ControlPanel : BasePage
{
    private bool _isPopupOpen = false;
    private WidgetType? _lastWidgetCallingPopup = null;

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
                _lastWidgetCallingPopup = e.Type;
                break;
            case WidgetAction.ClosePopup:
                _isPopupOpen = false;
                _lastWidgetCallingPopup = null;
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
        // Turn off scrolling when a popup is open, to avoid accidental navigation.
        if (_isPopupOpen)
        {
            return;
        }

        if (e.DeltaY < 0)
        {
            NavigationManager.NavigateTo(Destination);
        }
    }

    public override void OnKeyboardKeyClicked(KeyboardEventArgs e)
    {
        // Turn off keyboard navigation when a popup is open, to avoid accidental navigation.
        if (_isPopupOpen)
        {
            return;
        }

        switch (e.Key)
        {
            case "ArrowUp":
                NavigationManager.NavigateTo(Destination);
                break;
        }
    }
}
