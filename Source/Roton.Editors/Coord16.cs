using System.ComponentModel.DataAnnotations;
using Roton.Emulation.Data;

namespace Roton.Editors;

/// <summary>
/// Represents a coordinate pair.
/// </summary>
public struct Coord16
{
    public static implicit operator Coord16(Location16 val) => new(val.X, val.Y);
    public static implicit operator Coord16(Vector val) => new(val.X, val.Y);

    public static implicit operator Location16(Coord16 val) => new(val.X, val.Y);
    public static implicit operator Vector(Coord16 val) => new(val.X, val.Y);

    public Coord16(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coord16(Location location)
    {
        X = location.X;
        Y = location.Y;
    }

    public Coord16(Vector vector)
    {
        X = vector.X;
        Y = vector.Y;
    }

    [Range(short.MinValue, short.MaxValue)]
    public int X { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Y { get; set; }
}