using System.ComponentModel.DataAnnotations;

namespace Roton.Editors;

/// <summary>
/// Represents a tile+color pair.
/// </summary>
public struct Tile
{
    public static implicit operator Roton.Emulation.Data.Tile(Tile val) => new(val.Id, val.Color);
    public static implicit operator Tile(Roton.Emulation.Data.Tile val) => new(val.Id, val.Color);

    public Tile(int id, int color)
    {
        Id = id;
        Color = color;
    }

    [Range(byte.MinValue, byte.MaxValue)] public int Id { get; set; }
    [Range(byte.MinValue, byte.MaxValue)] public int Color { get; set; }
}