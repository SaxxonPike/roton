using System;

namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryTimer : ITimer
    {
        private readonly Memory<byte> _memory;

        internal MemoryTimer(IMemory memory, int offset)
        {
            _memory = memory.Slice(offset);
        }

        public int Ticks
        {
            get => _memory.Read16();
            set => _memory.Write16(value);
        }
    }
}