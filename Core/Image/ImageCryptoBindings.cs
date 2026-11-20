using System.Runtime.InteropServices;

namespace Core.Image;

internal static class ImageCryptoBindings
{
    private const string LibraryName = "libimagecrypto";

    internal enum CryptoStatus : int
    {
        Success = 0,
        InvalidArgument = 1,
        InvalidKey = 2,
        OutOfMemory = 3,
        InternalError = 4,
    }

    internal enum CryptoPixelFormat : int
    {
        RGB = 3,
        RGBA = 4,
    }

    [DllImport(
        LibraryName,
        EntryPoint = "crypto_encrypt_1",
        CallingConvention = CallingConvention.Cdecl
    )]
    internal static extern CryptoStatus Encrypt_1(
        byte[] pixels,
        UIntPtr size,
        uint width,
        uint height,
        CryptoPixelFormat format,
        [Out] double[] key
    );

    [DllImport(
        LibraryName,
        EntryPoint = "crypto_decrypt_1",
        CallingConvention = CallingConvention.Cdecl
    )]
    internal static extern CryptoStatus Decrypt_1(
        byte[] pixels,
        UIntPtr size,
        uint width,
        uint height,
        CryptoPixelFormat format,
        [In] double[] key
    );

    [DllImport(
        LibraryName,
        EntryPoint = "crypto_encrypt_2",
        CallingConvention = CallingConvention.Cdecl
    )]
    internal static extern CryptoStatus Encrypt_2(byte[] pixels, UIntPtr size, [Out] double[] key);

    [DllImport(
        LibraryName,
        EntryPoint = "crypto_decrypt_2",
        CallingConvention = CallingConvention.Cdecl
    )]
    internal static extern CryptoStatus Decrypt_2(byte[] pixels, UIntPtr size, [In] double[] key);
}
