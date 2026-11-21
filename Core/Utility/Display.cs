using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Utility;

public static class Display
{
    public static string TrimString(string text, int maxLength)
    {
        if (text.Length <= maxLength)
            return text;

        int trimLength = maxLength - 3;
        if (trimLength <= 0)
        {
            return "...";
        }

        return text.Substring(0, trimLength) + "...";
    }
}
