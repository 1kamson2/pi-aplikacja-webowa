using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utility;

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
