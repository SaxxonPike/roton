using System;
using System.IO;

namespace Roton
{
    internal static class Exceptions
    {
        public static readonly Exception CorruptedData = new InvalidOperationException(
            "The data appears to be corrupt."
            );

        public static readonly Exception DataTooLarge = new InvalidOperationException(
            "There is too much data."
            );

        public static readonly Exception InvalidFormat = new InvalidDataException(
            "The data is in an invalid format."
            );

        public static readonly Exception InvalidSet = new InvalidOperationException(
            "Can't set this property."
            );

        public static readonly Exception Packed = new InvalidOperationException(
            "The board is currently packed; Unpack it to the context first before accessing it."
            );

        public static readonly Exception PushStackOverflow = new StackOverflowException(
            "A push was initiated with a zero vector. If allowed to continue, this would cause a stack overflow."
            );

        public static readonly Exception TooManyActors = new InvalidOperationException(
            "A new actor cannot be added to the board because it is at the maximum number of actors."
            );

        public static readonly Exception UnknownFormat = new InvalidDataException(
            "The data is in an unknown format."
            );
    }
}