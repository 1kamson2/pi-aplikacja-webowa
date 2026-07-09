using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
namespace Core.Components.Pages;


public abstract partial class PageBase : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    public required string Destination { get; init; }

    public PageBase(string destination)
    {
        Destination = destination;
    }
    public virtual void OnMouseScroll(WheelEventArgs e) => NavigationManager.NavigateTo(Destination);
}
