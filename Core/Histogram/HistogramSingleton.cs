namespace Core.Histogram;

using Core.Singleton;

public record HistogramEntry(
    Guid Id,
    DateTime CreatedAt,
    string FileName,
    string Source,
    int[] Red,
    int[] Green,
    int[] Blue
);

public class HistogramSingleton : Singleton<HistogramSingleton>, ISingletonStore<HistogramEntry>
{
    public ushort MaxEntries { get; } = ushort.MaxValue;
    private readonly List<HistogramEntry> _entries = new();
    public IReadOnlyList<HistogramEntry> Entries
    {
        get
        {
            lock (_lock)
                return _entries.ToList();
        }
    }

    public void Add(HistogramEntry entry)
    {
        lock (_lock)
            _entries.Insert(0, entry);
    }

    public void Remove(Guid id)
    {
        lock (_lock)
            _entries.RemoveAll(e => e.Id == id);
    }

    public void Clear()
    {
        lock (_lock)
            _entries.Clear();
    }
}
