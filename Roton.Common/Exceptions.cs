using System;

namespace Roton.Common
{
    internal static class Exceptions
    {
        public static Exception InvalidFont = new FormatException(
            "The font format is invalid."
            );

        public static Exception InvalidPalette = new FormatException(
            "The palette format is invalid."
            );
    }
}