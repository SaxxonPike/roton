using System;

namespace Roton.Common
{
    internal static class Exceptions
    {
        public static readonly Exception InvalidFont = new FormatException(
            "The font format is invalid."
            );

        public static readonly Exception InvalidPalette = new FormatException(
            "The palette format is invalid."
            );
    }
}