using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data;

public interface IActor : IExecutable, IProgrammable
{
    ref Location Location { get; }
    ref Tile UnderTile { get; }
    ref Vector Vector { get; }
    int Cycle { get; set; }
    int Follower { get; set; }
    int Leader { get; set; }
    int Length { get; set; }
    int P1 { get; set; }
    int P2 { get; set; }
    int P3 { get; set; }
    int Pointer { get; set; }
}