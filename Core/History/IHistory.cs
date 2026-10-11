namespace Core.History;

public interface IHistory
{
    IReadOnlyList<EncryptionEntry> Entries { get; }   // najnowsze na górze
    event Action? Changed;                            // komponenty odświeżają się po zmianie

    void Add(EncryptionEntry entry);
    void Remove(Guid id);
    void Clear();
}