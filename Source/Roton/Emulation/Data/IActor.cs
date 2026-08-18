using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data;

public interface IActor : IProgrammable
{
    ref Location Location { get; }
    ref Tile UnderTile { get; }
    ref Vector Vector { get; }
    ref Word Cycle { get; }
    ref Word Follower { get; }
    ref Word Leader { get; }
    ref Word Instruction { get; }
    ref Word Length { get; }
    ref byte P1 { get; }
    ref byte P2 { get; }
    ref byte P3 { get; }
    ref DWord Pointer { get; }
}