using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Mapping
{
    internal sealed class MemoryLocation16 : Location
    {
        public MemoryLocation16(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        private IMemory Memory { get; }

        private int Offset { get; }

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