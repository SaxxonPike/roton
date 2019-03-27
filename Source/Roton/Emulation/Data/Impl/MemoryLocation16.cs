using System;

namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryLocation16 : IXyPair
    {
        private readonly Memory<byte> _memory;

        internal MemoryLocation16(IMemory memory, int offset)
        {
            _memory = memory.Slice(offset);
        }

        public IXyPair Clone()
        {
            return new Location(X, Y);
        }

        public int X
        {
            get => _memory.Read16(0x00);
            set => _memory.Write16(0x00, value);
        }

        public int Y
        {
            get => _memory.Read16(0x02);
            set => _memory.Write16(0x02, value);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}