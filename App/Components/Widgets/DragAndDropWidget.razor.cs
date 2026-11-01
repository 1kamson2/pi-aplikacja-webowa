using System.Diagnostics;
using Core.Image.Processing;
using Core.Utility;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace App.Components.Widgets;

public partial class DragAndDropWidget : Widget
{
    private const float MaxFileSizeInMB = 5;
    private readonly float MaxFileSizeInBytes = UtilityFile.Convert(
        UtilityFile.SizeUnits.MegaByte,
        UtilityFile.SizeUnits.Byte,
        MaxFileSizeInMB
    );
    public static FileMetadata? FileMetadata { get; private set; }
    public static ImageMetadata? ImageMetadata { get; private set; }

    private enum SlideKind
    {
        Original,
        Encrypted,
        Decrypted,
    }

    private SlideKind _slide = SlideKind.Original;
    private int _lastSlideCount;
    private DateTime _lastWheel = DateTime.MinValue;
    private readonly Dictionary<SlideKind, (byte[] Ref, string Url)> _urlCache = new();

    private List<(SlideKind Kind, string Label, byte[] Data)> Slides()
    {
        var list = new List<(SlideKind, string, byte[])>();
        if (FileMetadata?.FiledataBlob is { Length: > 0 } o)
            list.Add((SlideKind.Original, "Oryginał", o));
        if (FileMetadata?.FiledataEncryptedBlob is { Length: > 0 } e)
            list.Add((SlideKind.Encrypted, "Zaszyfrowany", e));
        return list;
    }

    private void SyncSlide(List<(SlideKind Kind, string Label, byte[] Data)> slides)
    {
        if (slides.Count > _lastSlideCount && slides.Count > 1)
            _slide = slides[^1].Kind;
        else if (slides.Count > 0 && !slides.Exists(s => s.Kind == _slide))
            _slide = slides[0].Kind;
        _lastSlideCount = slides.Count;
    }

    private void SelectSlide(SlideKind kind) => _slide = kind;

    private void OnWheel(WheelEventArgs e)
    {
        var slides = Slides();
        if (slides.Count < 2)
            return;

        var now = DateTime.UtcNow;
        if ((now - _lastWheel).TotalMilliseconds < 450)
            return;
        _lastWheel = now;

        var i = Math.Max(0, slides.FindIndex(s => s.Kind == _slide));
        i = Math.Clamp(i + (e.DeltaY > 0 ? 1 : -1), 0, slides.Count - 1);
        _slide = slides[i].Kind;
    }

    private string UrlFor(SlideKind kind, byte[] data)
    {
        if (_urlCache.TryGetValue(kind, out var c) && ReferenceEquals(c.Ref, data))
            return c.Url;
        var url = "data:image/png;base64," + Convert.ToBase64String(data);
        _urlCache[kind] = (data, url);
        return url;
    }

    public static bool ChangeFileMetadata(FileMetadata fileMetadata)
    {
        FileMetadata = fileMetadata;
        return true;
    }

    public static bool ChangeImageMetadata(ImageMetadata imageMetadata)
    {
        ImageMetadata = imageMetadata;
        return true;
    }

    public override bool HasContent() =>
        FileMetadata?.FiledataBlob?.Length > 0 || FileMetadata?.FiledataEncryptedBlob?.Length > 0;

    private float ImageSizeInMB() =>
        UtilityFile.Convert(
            UtilityFile.SizeUnits.Byte,
            UtilityFile.SizeUnits.MegaByte,
            FileMetadata?.FiledataBlob?.Length ?? 0
        );

    private async Task OnFileDeleted(MouseEventArgs e)
    {
        FileMetadata = null;
        ImageMetadata = null;
        await Task.CompletedTask;
    }

    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        if (e.FileCount > 1)
        {
            NotificationsService.Error("Wybierz jeden plik z obrazem.");
            return;
        }

        try
        {
            using MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream((long)MaxFileSizeInBytes).CopyToAsync(ms);
            FileMetadata = new FileMetadata.FileMetadataBuilder()
                .WithFilename(e.File.Name)
                .WithFiledataBlob(ms.ToArray())
                .Build();
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidDataException)
        {
            NotificationsService.Error($"Nieobsługiwany typ obrazu.", "Obsługiwane typy to PNG");
            Debug.WriteLine(ex.Message);
        }
        catch (IOException ex)
        {
            NotificationsService.Error(
                "Plik jest za duży.",
                $"Maksymalny rozmiar pliku nie może przekraczać {MaxFileSizeInMB} megabajtów."
            );
            Debug.WriteLine(ex.Message);
        }
    }
}
