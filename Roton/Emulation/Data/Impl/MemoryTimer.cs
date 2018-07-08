namespace Roton.Emulation.Data.Impl
{
    public class MemoryTimer : ITimer
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        public MemoryTimer(IMemory memory, int offset)
        {
            _memory = memory;
            _offset = offset;
        }

        public int Ticks
        {
            get { return _memory.Read16(_offset); }
            set { _memory.Write16(_offset, value); }
        }
    }
}