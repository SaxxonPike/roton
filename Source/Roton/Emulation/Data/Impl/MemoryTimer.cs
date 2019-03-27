namespace Roton.Emulation.Data.Impl
{
    public sealed class MemoryTimer : ITimer
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        internal MemoryTimer(IMemory memory, int offset)
        {
            _memory = memory;
            _offset = offset;
        }

        public int Ticks
        {
            get => _memory.Read16(_offset);
            set => _memory.Write16(_offset, value);
        }
    }
}