using System;

namespace Roton.Emulation.Infrastructure;

public static class Exceptions
{
    public static readonly Exception CorruptedData = new(
        "The data appears to be corrupt."
    );

    public static readonly Exception DataTooLarge = new(
        "The data is too large to be stored in this format. Consider simplifying it."
    );

    public static readonly Exception InvalidFormat = new(
        "The data is in an invalid format."
    );

    public static readonly Exception InvalidSet = new(
        "Can't set this property."
    );

    public static readonly Exception PushStackOverflow = new(
        "Pushing requires a non-zero vector."
    );

    public static readonly Exception SelfReferenceCentipede = new(
        "Centipede segments must not self-reference."
    );

    public static readonly Exception UnknownFormat = new(
        "The data is in an unknown format."
    );
}