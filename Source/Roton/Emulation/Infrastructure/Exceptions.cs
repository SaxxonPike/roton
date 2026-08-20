using System;

namespace Roton.Emulation.Infrastructure;

public static class Exceptions
{
    public static RotonException CorruptedData => new(
        "The data appears to be corrupt."
    );

    public static RotonException DataTooLarge => new(
        "The data is too large to be stored in this format. Consider simplifying it."
    );

    public static RotonException InvalidSet => new(
        "Can't set this property."
    );

    public static RotonException PushStackOverflow => new(
        "Pushing requires a non-zero vector."
    );

    public static RotonException SelfReferenceCentipede => new(
        "Centipede segments must not self-reference."
    );
}