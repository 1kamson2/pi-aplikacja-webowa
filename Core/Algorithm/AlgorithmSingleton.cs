namespace Core.Algorithm;

using Core.Singleton;

public enum AlgorithmId : int
{
    ChaoticCircularShifts = 1,
    LTS = 2,
}

public record AlgorithmEntry(string Name, string Description, AlgorithmId Id);

public sealed class AlgorithmSingleton
    : Singleton<AlgorithmSingleton>,
        ISingletonStore<AlgorithmEntry>
{
    public ushort MaxEntries { get; } = ushort.MaxValue;
    private readonly List<AlgorithmEntry> _entries = new()
    {
        new AlgorithmEntry("Chaotic Circular Shifts", "Algorytm opracowany na podstawie pracy: Exploiting Circular Shifts for Efficient Chaotic\r\nImage Encryption", AlgorithmId.ChaoticCircularShifts),
        new AlgorithmEntry("LTS", "Algortym autorski", AlgorithmId.LTS),
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

    public bool Add(AlgorithmEntry entry)
    {
        lock (_lock)
            _entries.Add(entry);
        return true;
    }

    public bool Remove(string hash)
    {
        lock (_lock)
            return _entries.RemoveAll(e => e.Name == hash) > 0;
    }

    public bool Clear()
    {
        lock (_lock)
            _entries.Clear();
        return true;
    }
}
