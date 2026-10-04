using Microsoft.AspNetCore.Components.Web;

namespace App.Components.Pages;

using Shared;

public partial class Home : BasePage
{
    public Home()
    {
        Destination = "/control-panel";
    }

    public override void OnMouseScroll(WheelEventArgs e)
    {
        if (e.DeltaY > 0)
        {
            NavigationManager.NavigateTo(Destination);
        }
    }

    public override void OnKeyboardKeyClicked(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "ArrowDown":
                NavigationManager.NavigateTo(Destination);
                break;
        }
    }
}
