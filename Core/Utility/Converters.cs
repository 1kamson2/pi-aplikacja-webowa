using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utility;

internal static class Converters
{
    public static byte[] FromDoubleArrayToByteArray(double[] from)
    {
        byte[] to = new byte[from.Length * sizeof(double)];
        Buffer.BlockCopy(from, 0, to, 0, to.Length);
        return to;
    }
}
