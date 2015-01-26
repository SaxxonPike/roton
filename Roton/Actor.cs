using Roton.Emulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Roton
{
    public partial class Actor
    {
        virtual public int Cycle { get; set; }
        virtual public int Follower { get; set; }
        virtual internal Heap Heap { get; set; }
        virtual public int Instruction { get; set; }
        virtual public int Leader { get; set; }
        virtual public int Length { get; set; }
        virtual public Location Location { get; set; }
        virtual public int P1 { get; set; }
        virtual public int P2 { get; set; }
        virtual public int P3 { get; set; }
        virtual public int Pointer { get; set; }
        virtual public Tile UnderTile { get; set; }
        virtual public Vector Vector { get; set; }
    }
}
