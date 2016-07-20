using System;
using System.IO;

namespace Roton
{
    internal static class Exceptions
    {
        public static readonly Exception CorruptedData = new Exception(
            "The data appears to be corrupt."
            );

        public static readonly Exception DataTooLarge = new Exception(
            "The data is too large to be stored in this format. Consider simplifying it."
            );

        public static readonly Exception InvalidFormat = new Exception(
            "The data is in an invalid format."
            );

        public static readonly Exception InvalidSet = new Exception(
            "Can't set this property."
            );

        public static readonly Exception PushStackOverflow = new Exception(
            "Pushing requires a non-zero vector."
            );

        public static readonly Exception UnknownFormat = new Exception(
            "The data is in an unknown format."
            );

        public static readonly Exception SelfReferenceCentipede = new Exception(
            "Centipede segments must not self-reference."
            );
    }
}