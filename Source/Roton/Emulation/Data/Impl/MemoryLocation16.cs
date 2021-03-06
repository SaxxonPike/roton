﻿namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryLocation16 : IXyPair
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        internal MemoryLocation16(IMemory memory, int offset)
        {
            _memory = memory;
            _offset = offset;
        }

        public IXyPair Clone()
        {
            return new Location(X, Y);
        }

        public int X
        {
            get => _memory.Read16(_offset + 0x00);
            set => _memory.Write16(_offset + 0x00, value);
        }

        public int Y
        {
            get => _memory.Read16(_offset + 0x02);
            set => _memory.Write16(_offset + 0x02, value);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}