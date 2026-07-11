using Microsoft.AspNetCore.Components;

namespace Core.Components;

public struct ButtonPayload : IWidgetPayload
{
    public WidgetType Type { get; set; }
    public WidgetAction Action { get; set; }
    public int Id { get; set; }
    public RenderFragment? Content { get; set; }
};

public abstract partial class BaseButton<PayloadType> : BaseWidget<PayloadType> where PayloadType : IWidgetPayload, new()
{
    [Parameter] public required int Id { get; init; }
}