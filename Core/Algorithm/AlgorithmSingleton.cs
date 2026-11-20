namespace Core.Algorithm;

using Core.Singleton;

public enum AlgorithmId : int
{
    Algorithm1 = 1,
    Algorithm2 = 2,
}

public record AlgorithmEntry(string Name, string Description, AlgorithmId Id);

public sealed class AlgorithmSingleton
    : Singleton<AlgorithmSingleton>,
        ISingletonStore<AlgorithmEntry>
{
    public ushort MaxEntries { get; } = ushort.MaxValue;
    private readonly List<AlgorithmEntry> _entries = new()
    {
        new AlgorithmEntry("Algorytm 1", "Opis algorytmu 1", AlgorithmId.Algorithm1),
        new AlgorithmEntry("Algorytm 2", "Opis algorytmu 2", AlgorithmId.Algorithm2),
    };
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

    public void SelectAlgorithm(string name)
    {
        lock (_lock)
            SelectedAlgorithm = _entries.FirstOrDefault(e => e.Name == name);
    }

    public void Add(AlgorithmEntry entry)
    {
        lock (_lock)
            _entries.Add(entry);
    }

    public void Remove(string hash)
    {
        return;
    }

    public void Clear()
    {
        lock (_lock)
            _entries.Clear();
    }
}
