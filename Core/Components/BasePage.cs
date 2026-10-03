using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
namespace Core.Components;


public abstract partial class BasePage : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public required string Destination { get; init; }

    public virtual void OnMouseScroll(WheelEventArgs e) => NavigationManager.NavigateTo(Destination);
}
