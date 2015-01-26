using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton
{
    static internal class Exceptions
    {
        static public Exception CorruptedData = new InvalidOperationException(
            "The data appears to be corrupt."
            );

        static public Exception DataTooLarge = new InvalidOperationException(
            "There is too much data."
            );

        static public Exception InvalidFormat = new InvalidDataException(
            "The data is in an invalid format."
            );

        static public Exception InvalidSet = new InvalidOperationException(
            "Can't set this property."
            );

        static public Exception Packed = new InvalidOperationException(
            "The board is currently packed; Unpack it to the context first before accessing it."
            );

        static public Exception TooManyActors = new InvalidOperationException(
            "A new actor cannot be added to the board because it is at the maximum number of actors."
            );

        static public Exception UnknownFormat = new InvalidDataException(
            "The data is in an unknown format."
            );
    }
}
