namespace Roton.Emulation.Data.Impl;

/// <summary>
/// An unsigned 8-bit (X,Y) pair.
/// </summary>
public sealed class Location : IXyPair
{
    public Location()
    {
    }

    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public IXyPair Clone() => 
        new Location(X, Y);

    public int X
    {
        get;
        set => field = value & 0xFF;
    }

    public int Y
    {
        get;
        set => field = value & 0xFF;
    }

    public override string ToString() => 
        $"({X}, {Y})";
}