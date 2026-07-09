namespace Core.Utils;

public static class Elements
{
    public static class Button
    {
        public enum Type
        {
            SaveImage = 0,
            EncryptionHistory,
            Histograms,
            Algorithms
        }
        public record Information(Type Type, int Id, string Content);
    }
}
