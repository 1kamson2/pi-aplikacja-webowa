using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
namespace Core.Components.Pages;


public partial class BasePage : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    public required string Destination { get; init; }

    public BasePage(string destination)
    {
        Destination = destination;
    }
    public virtual void OnMouseScroll(WheelEventArgs e) => NavigationManager.NavigateTo(Destination);
}
