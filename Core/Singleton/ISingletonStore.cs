namespace Core.Singleton;

public interface ISingletonStore<TEntries>
{
    public ushort MaxEntries { get; }
    public IReadOnlyList<TEntries> Entries { get; }
    public abstract void Add(TEntries entry);
    public abstract void Remove(Guid id);
    public abstract void Clear();
}
