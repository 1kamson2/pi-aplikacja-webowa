using System.Runtime.InteropServices;

namespace Core.Image;

internal static class ImageCryptoBindings
{
    private const string Dll = "libimagecrypto.dll";

    internal enum CryptoStatus : int
    {
        Success = 0,
        InvalidArgument = 1,
        OutOfMemory = 2,
        InternalError = 3,
    }

    internal enum CryptoPixelFormat : int
    {
        RGB = 3,
        RGBA = 4,
    }

    [DllImport(Dll, EntryPoint = "crypto_encrypt", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CryptoStatus Encrypt(
        byte[] fileData,
        UIntPtr size,
        uint width,
        uint height,
        CryptoPixelFormat format,
        [Out] double[] key
    );

    [DllImport(Dll, EntryPoint = "crypto_decrypt", CallingConvention = CallingConvention.Cdecl)]
    internal static extern CryptoStatus Decrypt(
        byte[] fileData,
        UIntPtr size,
        uint width,
        uint height,
        CryptoPixelFormat format,
        [In] double[] key
    );
}
