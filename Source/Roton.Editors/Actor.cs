using System.ComponentModel.DataAnnotations;

namespace Roton.Editors;

public class Actor
{
    public Coord Location { get; set; }
    public Tile UnderTile { get; set; }
    public Coord16 Vector { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Cycle { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Follower { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Leader { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Instruction { get; set; }

    [Range(short.MinValue, short.MaxValue)]
    public int Length { get; set; }

    [Range(byte.MinValue, byte.MaxValue)] public int P1 { get; set; }
    [Range(byte.MinValue, byte.MaxValue)] public int P2 { get; set; }
    [Range(byte.MinValue, byte.MaxValue)] public int P3 { get; set; }

    [Range(int.MinValue, int.MaxValue)] public int Pointer { get; set; }
    public Script? Script { get; set; }

    public byte[] Reserved { get; set; } = [];
}