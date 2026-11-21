namespace Core.History;

using Core.Services;
using Core.Singleton;
using Core.Utility;

public class HistorySingleton : Singleton<HistorySingleton>, ISingletonStore<ImageMetadata>
{
    public ushort MaxEntries { get; } = ushort.MaxValue;
    public HistoryModel Model { get; } = new HistoryModel();
    private List<ImageMetadata> _entries = new();

    protected override void Setup()
    {
        _entries = Model.ExecuteSelect("SelectAllEncryptionHistory");
    }

    public IReadOnlyList<ImageMetadata> Entries
    {
        get
        {
            lock (_lock)
            {
                return _entries.ToList();
            }
        }
    }

    public bool Add(ImageMetadata entry)
    {
        lock (_lock)
        {
            _entries.Insert(0, entry);
            if (_entries.Count > MaxEntries)
            {
                _entries.RemoveAt(_entries.Count - 1);
            }
            Model.ExecuteInsert("InsertEncryptionHistory", entry);
        }
        return true;
    }

    public bool Remove(string hash)
    {
        lock (_lock)
        {
            _entries.RemoveAll(e => e.ImageHash == hash);
            return Model.ExecuteDelete("DeleteEncryptionHistory", hash);
        }
    }

    public bool Clear()
    {
        lock (_lock)
        {
            _entries.Clear();
        }
        return true;
    }
}
