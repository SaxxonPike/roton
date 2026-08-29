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
}