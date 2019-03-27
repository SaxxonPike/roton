using System;

namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryLocation : IXyPair
    {
        private readonly Memory<byte> _memory;

        internal MemoryLocation(IMemory memory, int offset)
        {
            _memory = memory.Slice(offset);
        }

        public IXyPair Clone()
        {
            return new Location(X, Y);
        }

        public int X
        {
            get => _memory.Span[0x00];
            set => _memory.Write8(0x00, value);
        }

        public int Y
        {
            get => _memory.Span[0x01];
            set => _memory.Write8(0x01, value);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}