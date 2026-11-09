namespace Core.Singleton;
/// <summary>
/// Storage for singleton
/// </summary>
/// <typeparam name="TEntries">Type of the item stored</typeparam>
public interface ISingletonStore<TEntries>
{
    public ushort MaxEntries { get; }
    public abstract void Add(TEntries entry);
    public abstract void Remove(string hash);
    public abstract void Clear();
}
