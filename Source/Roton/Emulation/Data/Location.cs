using System;

namespace Roton.Emulation.Data;

/// <summary>
/// An unsigned 8-bit (X,Y) pair.
/// </summary>
public struct Location : IEquatable<Location>
{
    public static Location operator +(Location a, Vector b) => new(a.X + b.X, a.Y + b.Y);
    public static Location operator -(Location a, Vector b) => new(a.X - b.X, a.Y - b.Y);
    public static Location operator *(Location a, Vector b) => new(a.X * b.X, a.Y * b.Y);
    public static Location operator /(Location a, Vector b) => new(a.X / b.X, a.Y / b.Y);

    public static Location operator +(Location a, int b) => new(a.X + b, a.Y + b);
    public static Location operator -(Location a, int b) => new(a.X - b, a.Y - b);
    public static Location operator *(Location a, int b) => new(a.X * b, a.Y * b);
    public static Location operator /(Location a, int b) => new(a.X / b, a.Y / b);

    public static Vector operator -(Location a, Location b) => new(a.X - b.X, a.Y - b.Y);

    public static bool operator ==(Location a, Location b) => a.Equals(b);
    public static bool operator !=(Location a, Location b) => !a.Equals(b);
    
    private byte _x;
    private byte _y;

    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X
    {
        get => _x;
        set => _x = unchecked((byte)(value & 0xFF));
    }

    public int Y
    {
        get => _y;
        set => _y = unchecked((byte)(value & 0xFF));
    }

    public override string ToString() =>
        $"({X}, {Y})";

    public override bool Equals(object? obj)
    {
        if (obj is Location other)
            return Equals(other);
        return false;
    }

    public bool Equals(Location other) => 
        _x == other._x && _y == other._y;

    public override int GetHashCode() =>
        BitConverter.IsLittleEndian 
            ? ((_y << 8) | _x).GetHashCode() 
            : ((_x << 8) | _y).GetHashCode();
}