namespace Core.Components;

public static class Elements
{
    public static class Button
    {
        public enum Type
        {
            None = 0,
            Close,
            SaveImage,
            EncryptionHistory,
            Histograms,
            Algorithms
        }
        public record Information(Type Type, int Id, string Content);
    }
}
