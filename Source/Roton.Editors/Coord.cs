using System.ComponentModel.DataAnnotations;
using Roton.Emulation.Data;

namespace Roton.Editors;

/// <summary>
/// Represents a coordinate pair.
/// </summary>
public struct Coord
{
    public static implicit operator Coord(Location val) => new(val.X, val.Y);
    public static implicit operator Location(Coord val) => new(val.X, val.Y);

    public Coord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coord(Location location)
    {
        X = location.X;
        Y = location.Y;
    }

    public Coord(Vector vector)
    {
        X = vector.X;
        Y = vector.Y;
    }

    [Range(byte.MinValue, byte.MaxValue)] public int X { get; set; }
    [Range(byte.MinValue, byte.MaxValue)] public int Y { get; set; }
}