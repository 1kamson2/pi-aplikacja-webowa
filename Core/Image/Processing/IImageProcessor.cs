using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core.Image.Processing;

public enum ImageFormat
{
    Unknown,
    Jpg,
    Png,
}

public interface IImageProcessor<TSelf>
    where TSelf : IImageProcessor<TSelf>
{
    public static abstract TSelf Instance { get; }
    public abstract byte[] ImageSignature { get; }
    public abstract byte[] Process(byte[] imageData, string contentType);
    public abstract bool IsImage(string contentType);
    public abstract bool HasCorrectSignature(ReadOnlySpan<byte> bytes);
}
