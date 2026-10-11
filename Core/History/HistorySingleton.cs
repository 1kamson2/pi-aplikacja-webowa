namespace Core.History;

public static class HistorySingleton
{
    private static int MaxEntries = 20; 
    private static List<EncryptionEntry> _entries = new();
    private static readonly object _lock = new();

    public static event Action? Changed;

    public static IReadOnlyList<EncryptionEntry> Entries
    {
        get
        {
            lock (_lock)
                return _entries.ToList();
        } 
    }

    public static void Add(EncryptionEntry entry)
    {
        lock (_lock)
        {
            _entries.Insert(0, entry);
            if (_entries.Count > MaxEntries)
                _entries.RemoveAt(_entries.Count - 1);
        }
        Changed?.Invoke();
    }

    public static void Remove(Guid id)
    {
        lock (_lock)
            _entries.RemoveAll(e => e.Id == id);
        Changed?.Invoke();
    }

    public static void Clear()
    {
        lock (_lock)
            _entries.Clear();
        Changed?.Invoke();
    }
}
