namespace Core.Services;

public enum NotificationType
{
    Info,
    Success,
    Warning,
    Error
}

public sealed record Notification(
    Guid Id,
    string Title,
    string? Message,
    NotificationType Type,
    int DurationMs,
    DateTime Time
);

public sealed class NotificationsService
{
    private const int MaxHistory = 50;
    private readonly List<Notification> _history = new();

    public event Action<Notification>? OnShow;
    public event Action? OnChanged;

    public IReadOnlyList<Notification> History => _history;
    public int Unread { get; private set; }

    public void Show(string title, string? message = null,
        NotificationType type = NotificationType.Info, int durationMs = 2000)
    {
        var n = new Notification(Guid.NewGuid(), title, message, type, durationMs, DateTime.Now);
        _history.Insert(0, n);
        if (_history.Count > MaxHistory) _history.RemoveAt(_history.Count - 1);
        Unread++;
        OnShow?.Invoke(n);
    }

    public void MarkRead() => Unread = 0;

    public void Clear()
    {
        _history.Clear();
        Unread = 0;
        OnChanged?.Invoke();
    }

    public void Info(string title, string? message = null) =>
        Show(title, message, NotificationType.Info);

    public void Success(string title, string? message = null) =>
        Show(title, message, NotificationType.Success);

    public void Warning(string title, string? message = null) =>
        Show(title, message, NotificationType.Warning, 3000);

    public void Error(string title, string? message = null) =>
        Show(title, message, NotificationType.Error, 4000);
}