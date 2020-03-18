using System;

namespace Roton.Emulation.Infrastructure
{
    public static class Exceptions
    {
        public static Exception CorruptedData => new Exception(
            "The data appears to be corrupt."
            );

        public static Exception DataTooLarge => new Exception(
            "The data is too large to be stored in this format. Consider simplifying it."
            );

        public static Exception InvalidFormat => new Exception(
            "The data is in an invalid format."
            );

        public static Exception InvalidSet => new Exception(
            "Can't set this property."
            );

        public static Exception PushStackOverflow => new Exception(
            "Pushing requires a non-zero vector."
            );

        public static Exception SelfReferenceCentipede => new Exception(
            "Centipede segments must not self-reference."
            );

        public static Exception UnknownFormat => new Exception(
            "The data is in an unknown format."
            );
    }
}