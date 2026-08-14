namespace Roton.Emulation.Data.Impl;

/// <summary>
/// A signed 16-bit (X,Y) pair.
/// </summary>
public sealed class Location16 : IXyPair
{
    public Location16()
    {
    }

    public Location16(IXyPair source)
    {
        X = source.X;
        Y = source.Y;
    }

    public Location16(int x, int y)
    {
        X = x;
        Y = y;
    }

    public IXyPair Clone() => 
        new Location16(X, Y);

    public int X
    {
        get;
        set => field = (value << 16) >> 16;
    }

    public int Y
    {
        get;
        set => field = (value << 16) >> 16;
    }

    public override string ToString() => 
        $"({X}, {Y})";
}