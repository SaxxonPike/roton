using Roton.Core;

namespace Roton.Emulation
{
    internal sealed class MemoryLocation : Location
    {
        public MemoryLocation(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public Memory Memory { get; }

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