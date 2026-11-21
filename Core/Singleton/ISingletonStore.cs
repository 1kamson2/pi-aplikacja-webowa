namespace Core.Singleton;

/// <summary>
/// Storage for singleton
/// </summary>
/// <typeparam name="TEntries">Type of the item stored</typeparam>
public interface ISingletonStore<TEntries>
{
    public ushort MaxEntries { get; }
    public abstract bool Add(TEntries entry);
    public abstract bool Remove(string hash);
    public abstract bool Clear();
}
