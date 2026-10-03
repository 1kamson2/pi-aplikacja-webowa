using Core.Components;
using Microsoft.AspNetCore.Components.Web;

namespace App.Components.Pages;

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
}
