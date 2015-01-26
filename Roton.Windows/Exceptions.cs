using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Windows
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
