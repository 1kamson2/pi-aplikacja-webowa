namespace Core.Singleton;

public interface ISingletonStore<TEntries>
{
    public ushort MaxEntries { get; }
    public abstract void Add(TEntries entry);
    public abstract void Remove(string hash);
    public abstract void Clear();
}
