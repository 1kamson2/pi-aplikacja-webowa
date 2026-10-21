using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace Core.Utility;

public static class ApplicationPaths
{
    public static string ApplicationDatabasePath { get; set; } = "";
}

public enum ImageFormat
{
    Unknown,
    Jpg,
    Png,
}

public readonly record struct ImageDimensions(uint Width, uint Height);

public readonly record struct DecryptionKey(double[] Value);

/// <summary>
/// FileMetadata defines the metadata for the file.
/// </summary>
/// <param name="Filename">Filename</param>
/// <param name="Path">Path of the file</param>
/// <param name="Dimensions">Image dimensions (Width, Height)</param>
/// <param name="FiledataBlob">Contents of the file before encryption - must be decoded</param>
/// <param name="HasAlpha">If true image is RGBA</param>
public class FileMetadata
{
    public string? Filename { get; set; }
    public string? Path { get; set; }
    public byte[]? FiledataBlob { get; set; }
    public byte[]? FiledataEncryptedBlob { get; set; }

    public FileMetadata() { }

    public class FileMetadataBuilder
    {
        private readonly FileMetadata _fileMetadata = new FileMetadata();

        public FileMetadataBuilder FromImageMetadata(ImageMetadata imageMetadata)
        {
            _fileMetadata.Filename = imageMetadata.Filename;
            _fileMetadata.Path = imageMetadata.Path;
            _fileMetadata.FiledataBlob = imageMetadata.FiledataBlob;
            _fileMetadata.FiledataEncryptedBlob = imageMetadata.FiledataEncryptedBlob;
            return this;
        }

        public FileMetadataBuilder WithFilename(string filename)
        {
            _fileMetadata.Filename = filename;
            return this;
        }

        public FileMetadataBuilder WithPath(string path)
        {
            _fileMetadata.Path = path;
            return this;
        }

        public FileMetadataBuilder WithFiledataBlob(byte[] filedataBlob)
        {
            _fileMetadata.FiledataBlob = filedataBlob;
            return this;
        }

        public FileMetadataBuilder WithFiledataEncryptedBlob(byte[] filedataEncryptedBlob)
        {
            _fileMetadata.FiledataEncryptedBlob = filedataEncryptedBlob;
            return this;
        }

        public FileMetadata Build()
        {
            return _fileMetadata;
        }
    }
}

/// <summary>
/// ImageMetadata defines metadata for the Image.
/// </summary>
/// <param name="ImageHash">Hashed fields of the Image</param>
/// <param name="Filename">Filename</param>
/// <param name="Path">Path of the file</param>
/// <param name="Key">Decryption key for the image</param>
/// <param name="ImageFormat">Image format (JPG, PNG)</param>
/// <param name="Dimensions">Image dimensions (Width, Height)</param>
/// <param name="FiledataBlob">Contents of the file before encryption - must be decoded</param>
/// <param name="FiledataEncryptedBlob">Contents of the file after encryption - must be encoded</param>
/// <param name="HasAlpha">If true image is RGBA</param>
public sealed class ImageMetadata : FileMetadata
{
    public string? ImageHash { get; private set; }
    public DecryptionKey? Key { get; private set; }
    public ImageFormat? ImageFormat { get; private set; }
    public ImageDimensions? Dimensions { get; private set; }
    public bool HasAlpha { get; private set; } = false;

    public ImageMetadata()
        : base() { }

    public class ImageMetadataBuilder
    {
        private readonly ImageMetadata _imageMetadata = new ImageMetadata();

        public ImageMetadataBuilder FromFileMetadata(FileMetadata fileMetadata)
        {
            _imageMetadata.Filename = fileMetadata.Filename;
            _imageMetadata.Path = fileMetadata.Path;
            _imageMetadata.FiledataBlob = fileMetadata.FiledataBlob;
            _imageMetadata.FiledataEncryptedBlob = fileMetadata.FiledataEncryptedBlob;
            return this;
        }

        public ImageMetadataBuilder WithImageHash(string imageHash)
        {
            _imageMetadata.ImageHash = imageHash;
            return this;
        }

        public ImageMetadataBuilder WithFilename(string filename)
        {
            _imageMetadata.Filename = filename;
            return this;
        }

        public ImageMetadataBuilder WithPath(string path)
        {
            _imageMetadata.Path = path;
            return this;
        }

        public ImageMetadataBuilder WithFiledataBlob(byte[] filedataBlob)
        {
            _imageMetadata.FiledataBlob = filedataBlob;
            return this;
        }

        public ImageMetadataBuilder WithFiledataEncryptedBlob(byte[] filedataEncryptedBlob)
        {
            _imageMetadata.FiledataEncryptedBlob = filedataEncryptedBlob;
            return this;
        }

        public ImageMetadataBuilder WithKey(DecryptionKey key)
        {
            _imageMetadata.Key = key;
            return this;
        }

        public ImageMetadataBuilder WithImageFormat(ImageFormat imageFormat)
        {
            _imageMetadata.ImageFormat = imageFormat;
            return this;
        }

        public ImageMetadataBuilder WithImageFormat(string imageFormat)
        {
            ImageFormat eImageFormat = Enum.Parse<ImageFormat>(imageFormat);
            _imageMetadata.ImageFormat = eImageFormat;
            return this;
        }

        public ImageMetadataBuilder WithDimensions(ImageDimensions dimensions)
        {
            _imageMetadata.Dimensions = dimensions;
            return this;
        }

        public ImageMetadataBuilder WithHasAlpha()
        {
            _imageMetadata.HasAlpha = true;
            return this;
        }

        public ImageMetadataBuilder WithHasAlpha(bool hasAlpha)
        {
            _imageMetadata.HasAlpha = hasAlpha;
            return this;
        }

        public ImageMetadata Build()
        {
            string combinedFields =
                $"{_imageMetadata.Filename}|{_imageMetadata.ImageFormat}|{_imageMetadata.Dimensions}|{_imageMetadata.HasAlpha}|{_imageMetadata.FiledataEncryptedBlob}";
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(combinedFields);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            _imageMetadata.ImageHash = Convert.ToHexString(hashBytes).ToLowerInvariant();
            return _imageMetadata;
        }
    }
}

public static class UtilityFile
{
    public enum SizeUnits : uint
    {
        Byte = 1,
        KiloByte = 1024,
        MegaByte = KiloByte * 1024,
    }

    public static float Convert(SizeUnits from, SizeUnits to, float size)
    {
        float bytes = size * (float)from;
        return bytes / (float)to;
    }
}
