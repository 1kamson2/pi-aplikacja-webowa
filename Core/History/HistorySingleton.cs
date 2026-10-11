namespace Core.History;

using Core.Singleton;

public record EncryptionEntry(
    Guid Id,
    DateTime CreatedAt,
    string FileName,
    string Algorithm,
    byte[] OriginalData,
    byte[] EncryptedData
);

public class HistorySingleton : Singleton<HistorySingleton>, ISingletonStore<EncryptionEntry>
{
    public ushort MaxEntries { get; } = ushort.MaxValue;
    private readonly List<EncryptionEntry> _entries = new();
    public IReadOnlyList<EncryptionEntry> Entries
    {
        get
        {
            lock (_lock)
            {
                return _entries.ToList();
            }
        }
    }

    public void Add(EncryptionEntry entry)
    {
        lock (_lock)
        {
            _entries.Insert(0, entry);
            if (_entries.Count > MaxEntries)
            {
                _entries.RemoveAt(_entries.Count - 1);
            }
        }
    }

    public void Remove(Guid id)
    {
        lock (_lock)
        {
            _entries.RemoveAll(e => e.Id == id);
        }
        ;
    }

    public void Clear()
    {
        lock (_lock)
        {
            _entries.Clear();
        }
    }
}
