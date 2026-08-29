using System;

namespace Roton.Emulation.Data;

/// <summary>
/// A signed 16-bit (X,Y) pair.
/// </summary>
public struct Location16 : IEquatable<Location16>
{
    public static Location16 operator +(Location16 a, Vector b) => new(a.X + b.X, a.Y + b.Y);
    public static Location16 operator -(Location16 a, Vector b) => new(a.X - b.X, a.Y - b.Y);
    public static Location16 operator *(Location16 a, Vector b) => new(a.X * b.X, a.Y * b.Y);
    public static Location16 operator /(Location16 a, Vector b) => new(a.X / b.X, a.Y / b.Y);

    public static Location16 operator +(Location16 a, int b) => new(a.X + b, a.Y + b);
    public static Location16 operator -(Location16 a, int b) => new(a.X - b, a.Y - b);
    public static Location16 operator *(Location16 a, int b) => new(a.X * b, a.Y * b);
    public static Location16 operator /(Location16 a, int b) => new(a.X / b, a.Y / b);

    public static Vector operator -(Location16 a, Location16 b) => new(a.X - b.X, a.Y - b.Y);

    public static bool operator ==(Location16 a, Location16 b) => a.Equals(b);
    public static bool operator !=(Location16 a, Location16 b) => !a.Equals(b);

    private short _x;
    private short _y;

    public Location16(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X
    {
        get
        {
            if (BitConverter.IsLittleEndian)
                return _x;

            return ((_x & 0xFF) << 8) | ((_x >> 8) & 0xFF);
        }
        set
        {
            if (BitConverter.IsLittleEndian)
            {
                _x = unchecked((short)value);
                return;
            }

            _x = unchecked((short)(((value >> 8) & 0xFF) | ((value & 0xFF) << 8)));
        }
    }

    public int Y
    {
        get
        {
            if (BitConverter.IsLittleEndian)
                return _y;

            return ((_y & 0xFF) << 8) | ((_y >> 8) & 0xFF);
        }
        set
        {
            if (BitConverter.IsLittleEndian)
            {
                _y = unchecked((short)value);
                return;
            }

            _y = unchecked((short)(((value >> 8) & 0xFF) | ((value & 0xFF) << 8)));
        }
    }

    public override string ToString() =>
        $"({X}, {Y})";

    public override bool Equals(object? obj)
    {
        if (obj is Location16 other)
            return Equals(other);
        return false;
    }

    public bool Equals(Location16 other) =>
        _x == other._x && _y == other._y;

#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
    public override int GetHashCode() =>
        BitConverter.IsLittleEndian 
            ? ((_y << 16) | _x).GetHashCode() 
            : ((_x << 16) | _y).GetHashCode();
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
}