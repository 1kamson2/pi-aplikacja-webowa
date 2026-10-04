namespace Core.Algorithm;

using Core.Singleton;

public record AlgorithmEntry(
    Guid Id,
    string Name,
    string Category,
    string Description,
    int KeySizeBits,
    bool IsImplemented
);

public sealed class AlgorithmSingleton
    : Singleton<AlgorithmSingleton>,
        ISingletonStore<AlgorithmEntry>
{
    public ushort MaxEntries { get; } = ushort.MaxValue;
    private readonly List<AlgorithmEntry> _entries = new();
    public AlgorithmEntry? ChosenAlgorithm { get; private set; }

    public IReadOnlyList<AlgorithmEntry> Entries
    {
        get
        {
            lock (_lock)
                return _entries.ToList();
        }
    }

    public void SelectAlgorithm(Guid id)
    {
        lock (_lock)
            ChosenAlgorithm = _entries.FirstOrDefault(e => e.Id == id);
    }

    public void Add(AlgorithmEntry entry)
    {
        lock (_lock)
            _entries.Add(entry);
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
