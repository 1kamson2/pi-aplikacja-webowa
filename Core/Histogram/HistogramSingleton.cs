namespace Core.Histogram;

using System.Security.Cryptography;
using Core.History;
using Core.Singleton;
using Core.Utility;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using ImageSharp = SixLabors.ImageSharp.Image;

public struct HistogramCounts
{
    public int[] Red { get; set; } = new int[256];
    public int[] Green { get; set; } = new int[256];
    public int[] Blue { get; set; } = new int[256];

    public HistogramCounts() { }

    public HistogramCounts(int[] red, int[] green, int[] blue) =>
        (Red, Green, Blue) = (red, green, blue);

    public static HistogramCounts FromPixels(byte[] pixels)
    {
        HistogramCounts instance = new HistogramCounts();
        for (int i = 0; i < pixels.Length; i += 4)
        {
            instance.Red[pixels[i]]++;
            instance.Green[pixels[i + 1]]++;
            instance.Blue[pixels[i + 2]]++;
        }
        return instance;
    }
}

public record HistogramEntry(
    string ImageHash,
    string Filename,
    string Source,
    HistogramCounts HistogramCounts
);

public class HistogramSingleton : Singleton<HistogramSingleton>, ISingletonStore<ImageMetadata>
{
    public ushort MaxEntries { get; } = ushort.MaxValue;
    private List<HistogramEntry> _entries = new();
    public HistogramModel Model = new HistogramModel();

    protected override void Setup()
    {
        List<ImageMetadata> imageMetadataBuffer = Model.ExecuteSelect("SelectAllHistograms");
        foreach (var imageMetadata in imageMetadataBuffer)
        {
            (HistogramEntry originalEntry, HistogramEntry encodedEntry) =
                FromImageMetadataToHistogramEntry(imageMetadata);
            _entries.Insert(0, originalEntry);
            _entries.Insert(0, encodedEntry);
        }
    }

    public IReadOnlyList<HistogramEntry> Entries
    {
        get
        {
            lock (_lock)
                return _entries.ToList();
        }
    }

    private (
        HistogramEntry OriginalImage,
        HistogramEntry EncryptedImage
    ) FromImageMetadataToHistogramEntry(ImageMetadata imageMetadata)
    {
        string imageHash = imageMetadata.ImageHash;

        string originalFilename = imageMetadata.Filename;
        string encodedFilename = $"encoded-{imageMetadata.Filename}";

        string source = imageMetadata.Path;

        using var originalImg = ImageSharp.Load<Rgba32>(imageMetadata.FiledataBlob);
        using var encodedImg = ImageSharp.Load<Rgba32>(imageMetadata.FiledataEncryptedBlob);

        byte[] originalPixels = new byte[4 * originalImg.Width * originalImg.Height];
        byte[] encodedPixels = new byte[4 * encodedImg.Width * encodedImg.Height];

        originalImg.CopyPixelDataTo(originalPixels);
        encodedImg.CopyPixelDataTo(encodedPixels);

        HistogramCounts originalCounts = HistogramCounts.FromPixels(originalPixels);
        HistogramCounts encodedCounts = HistogramCounts.FromPixels(encodedPixels);

        return (
            new(imageHash, originalFilename, source, originalCounts),
            new(imageHash, encodedFilename, source, encodedCounts)
        );
    }

    public void Add(ImageMetadata entry)
    {
        lock (_lock)
        {
            (HistogramEntry originalEntry, HistogramEntry encodedEntry) =
                FromImageMetadataToHistogramEntry(entry);
            _entries.Insert(0, originalEntry);
            _entries.Insert(0, encodedEntry);
            Model.ExecuteInsert("InsertHistogram", entry);
        }
    }

    public void Remove(string hash)
    {
        lock (_lock)
            _entries.RemoveAll(e => e.ImageHash == hash);
    }

    public void Clear()
    {
        lock (_lock)
            _entries.Clear();
    }
}
