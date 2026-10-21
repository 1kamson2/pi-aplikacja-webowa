using Microsoft.AspNetCore.Components;

namespace App.Components.Widgets;

using Core.Histogram;
using Pages.Shared;

public partial class HistogramsPopupWidget : Widget
{
    [Parameter]
    public EventCallback<ButtonPayload> ButtonPushedEvent { get; set; }
    public HistogramSingleton HistogramSingleton { get; } = HistogramSingleton.Instance;

    public override bool HasContent() => HistogramSingleton.Entries.Count > 0;

    protected virtual async Task RelayButtonPayload(ButtonPayload e)
    {
        if (ButtonPushedEvent.HasDelegate)
        {
            await ButtonPushedEvent.InvokeAsync(e);
        }
    }

    private static string Points(int[] values, int max)
    {
        double m = max == 0 ? 1 : max;

        return string.Join(
            " ",
            values.Select(
                (v, i) =>
                {
                    double ratio = (double)v / m;
                    double normalized = Math.Pow(ratio, 0.2);
                    return FormattableString.Invariant($"{i},{100 - normalized * 100:0.##}");
                }
            )
        );
    }
}
