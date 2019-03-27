using System;

namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryTile : ITile
    {
        private readonly Memory<byte> _memory;

        internal MemoryTile(IMemory memory, int offset)
        {
            _memory = memory.Slice(offset);
        }

        internal MemoryTile(Memory<byte> memory)
        {
            _memory = memory;
        }

        public ITile Clone()
        {
            return new Tile(Id, Color);
        }

        public int Color
        {
            get => _memory.Span[0x01];
            set => _memory.Write8(0x01, value);
        }

        public int Id
        {
            get => _memory.Span[0x00];
            set => _memory.Write8(0x00, value);
        }

        public override string ToString()
        {
            return $"Id: {Id:x2}, Color: {Color:x2}";
        }
    }
}