using Microsoft.AspNetCore.Components;
using Core.Services;
namespace App.Components.Widgets;

public partial class NotificationWidget : ComponentBase, IDisposable
{
    private sealed class Entry
    {
        public Entry(Notification item) => Item = item;
        public Notification Item { get; }
        public bool Closing { get; set; }
    }

    private const int MaxVisible = 4;
    private readonly List<Entry> _items = new();
    private bool _open;
    private bool _disposed;

    protected override void OnInitialized()
    {
        NotificationsService.OnShow += Add;
        NotificationsService.OnChanged += Refresh;
    }

    private void Toggle()
    {
        _open = !_open;
        if (_open) NotificationsService.MarkRead();
    }

    private void Close() => _open = false;

    private void Refresh() => _ = InvokeAsync(() =>
    {
        if (!_disposed) StateHasChanged();
    });

    private void Add(Notification n) => _ = InvokeAsync(async () =>
    {
        if (_disposed) return;

        var entry = new Entry(n);
        _items.Add(entry);
        while (_items.Count > MaxVisible) _items.RemoveAt(0);
        if (_open) NotificationsService.MarkRead();
        StateHasChanged();

        await Task.Delay(n.DurationMs);
        await DismissAsync(entry);
    });

    private async Task DismissAsync(Entry entry)
    {
        if (_disposed || entry.Closing || !_items.Contains(entry)) return;

        entry.Closing = true;
        StateHasChanged();

        await Task.Delay(200);
        if (_disposed) return;

        _items.Remove(entry);
        StateHasChanged();
    }

    protected static string IconSvg(NotificationType type, string cssClass)
    {
        var paths = type switch
        {
            NotificationType.Success => "<path d='M20 6 9 17l-5-5'/>",
            NotificationType.Error => "<circle cx='12' cy='12' r='10'/><path d='m15 9-6 6M9 9l6 6'/>",
            NotificationType.Warning => "<path d='M10.29 3.86 1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z'/><path d='M12 9v4M12 17h.01'/>",
            _ => "<circle cx='12' cy='12' r='10'/><path d='M12 16v-4M12 8h.01'/>"
        };
        return $"<svg class='{cssClass}' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='1.75' stroke-linecap='round' stroke-linejoin='round' aria-hidden='true'>{paths}</svg>";
    }

    public void Dispose()
    {
        _disposed = true;
        NotificationsService.OnShow -= Add;
        NotificationsService.OnChanged -= Refresh;
    }
}