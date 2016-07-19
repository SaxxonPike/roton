﻿using Roton.Emulation;

namespace Roton.Core
{
    public interface IActor : ICodeSeekable, ICode
    {
        int Cycle { get; set; }
        int Follower { get; set; }
        int Leader { get; set; }
        int Length { get; set; }
        IXyPair Location { get; }
        int P1 { get; set; }
        int P2 { get; set; }
        int P3 { get; set; }
        int Pointer { get; set; }
        ITile UnderTile { get; }
        IXyPair Vector { get; }
        bool IsAttached { get; }
    }
}