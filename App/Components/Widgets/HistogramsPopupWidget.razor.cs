using Microsoft.AspNetCore.Components;

namespace App.Components.Widgets;

using Core.Histogram;
using Pages.Shared;

public partial class HistogramsPopupWidget : Widget
{
    [Parameter]
    public EventCallback<ButtonPayload> ButtonPushedEvent { get; set; }
    public HistogramSingleton HistogramSingleton { get; } = HistogramSingleton.Instance;

    protected virtual async Task RelayButtonPayload(ButtonPayload e)
    {
        if (ButtonPushedEvent.HasDelegate)
        {
            await ButtonPushedEvent.InvokeAsync(e);
        }
    }

    private static string Points(int[] values, int max) =>
        string.Join(
            " ",
            values.Select(
                (v, i) => FormattableString.Invariant($"{i},{100 - (double)v / max * 100:0.##}")
            )
        );
}
