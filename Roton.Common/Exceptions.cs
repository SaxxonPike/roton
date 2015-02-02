using System;

namespace Roton.Common
{
    static internal class Exceptions
    {
        static public Exception InvalidFont = new FormatException(
            "The font format is invalid."
            );

        static public Exception InvalidPalette = new FormatException(
            "The palette format is invalid."
            );
    }
}
