namespace Core.Algorithm;

using Core.Singleton;

public record AlgorithmEntry(
    string Hash,
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
    public AlgorithmEntry? SelectedAlgorithm { get; private set; }

    public string SelectedAlgorithmName() => SelectedAlgorithm?.Name ?? "Nie wybrano algorytmu";

    protected override void Setup()
    {
        return;
    }

    public IReadOnlyList<AlgorithmEntry> Entries
    {
        get
        {
            lock (_lock)
                return _entries.ToList();
        }
    }

    public void SelectAlgorithm(string hash)
    {
        lock (_lock)
            SelectedAlgorithm = _entries.FirstOrDefault(e => e.Hash == hash);
    }

    public void Add(AlgorithmEntry entry)
    {
        lock (_lock)
            _entries.Add(entry);
    }

    public void Remove(string hash)
    {
        lock (_lock)
            _entries.RemoveAll(e => e.Hash == hash);
    }

    public void Clear()
    {
        lock (_lock)
            _entries.Clear();
    }
}
