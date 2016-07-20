using Roton.Core;

namespace Roton.Emulation.Mapping
{
    internal sealed class MemoryLocation : Location
    {
        public MemoryLocation(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public IMemory Memory { get; }

        public int Offset { get; }

        public override int X
        {
            get { return Memory.Read8(Offset + 0x00); }
            set { Memory.Write8(Offset + 0x00, value); }
        }

        public override int Y
        {
            get { return Memory.Read8(Offset + 0x01); }
            set { Memory.Write8(Offset + 0x01, value); }
        }
    }
}