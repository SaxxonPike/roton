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
    ref HWord P1 { get; }
    ref HWord P2 { get; }
    ref HWord P3 { get; }
    ref DWord Pointer { get; }
}