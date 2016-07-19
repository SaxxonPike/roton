using Roton.Core;

namespace Roton.Emulation
{
    internal sealed class MemoryLocation16 : Location
    {
        public MemoryLocation16(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public Memory Memory { get; }

        public int Offset { get; }

        public override int X
        {
            get { return Memory.Read16(Offset + 0x00); }
            set { Memory.Write16(Offset + 0x00, value); }
        }

        public override int Y
        {
            get { return Memory.Read16(Offset + 0x02); }
            set { Memory.Write16(Offset + 0x02, value); }
        }
    }
}