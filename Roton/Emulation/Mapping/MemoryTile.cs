using Roton.Core;

namespace Roton.Emulation.Mapping
{
    internal sealed class MemoryTile : Tile
    {
        public MemoryTile(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override int Color
        {
            get { return Memory.Read8(Offset + 0x01); }
            set { Memory.Write8(Offset + 0x01, value); }
        }

        public override int Id
        {
            get { return Memory.Read8(Offset + 0x00); }
            set { Memory.Write8(Offset + 0x00, value); }
        }

        public IMemory Memory { get; }

        public int Offset { get; }
    }
}