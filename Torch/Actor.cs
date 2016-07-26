using Roton.Core;

namespace Torch
{
    internal class Actor : IActor
    {
        public int Cycle { get; set; }
        public int Follower { get; set; }
        public int Leader { get; set; }
        public int Length { get; set; }
        public IXyPair Location { get; } = new Location();
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public int Pointer { get; set; }
        public ITile UnderTile { get; } = new Tile();
        public IXyPair Vector { get; } = new Vector();
        public int Instruction { get; set; }
        public byte[] Code { get; set; }
    }
}