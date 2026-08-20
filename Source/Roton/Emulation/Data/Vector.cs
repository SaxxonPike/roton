using System;

namespace Roton.Emulation.Data;

/// <summary>
/// A signed 16-bit (X,Y) pair.
/// </summary>
public struct Vector : IEquatable<Vector>
{
    public static Vector operator -(Vector a) => new(-a.X, -a.Y);

    public static Vector operator +(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector operator -(Vector a, Vector b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector operator *(Vector a, Vector b) => new(a.X * b.X, a.Y * b.Y);
    public static Vector operator /(Vector a, Vector b) => new(a.X / b.X, a.Y / b.Y);

    public static Vector operator +(Vector a, int b) => new(a.X + b, a.Y + b);
    public static Vector operator -(Vector a, int b) => new(a.X - b, a.Y - b);
    public static Vector operator *(Vector a, int b) => new(a.X * b, a.Y * b);
    public static Vector operator /(Vector a, int b) => new(a.X / b, a.Y / b);

    public static bool operator ==(Vector a, Vector b) => a.Equals(b);
    public static bool operator !=(Vector a, Vector b) => !a.Equals(b);

    private short _x;
    private short _y;

    public static Vector East { get; } = new(1, 0);
    public static Vector Idle { get; } = new(0, 0);
    public static Vector North { get; } = new(0, -1);
    public static Vector South { get; } = new(0, 1);
    public static Vector West { get; } = new(-1, 0);

    public Vector(int x, int y)
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

    public bool IsNonZero() => X != 0 || Y != 0;
    public bool IsZero() => X == 0 && Y == 0;
    public Vector Swap() => new(Y, X);
    public Vector CounterClockwise() => new(Y, -X);
    public Vector Clockwise() => new(-Y, X);

    public override bool Equals(object? obj)
    {
        if (obj is Vector other)
            return Equals(other);
        return false;
    }

    public bool Equals(Vector other) =>
        _x == other._x && _y == other._y;

#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
    public override int GetHashCode() =>
        BitConverter.IsLittleEndian 
            ? ((_y << 16) | _x).GetHashCode() 
            : ((_x << 16) | _y).GetHashCode();
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
}