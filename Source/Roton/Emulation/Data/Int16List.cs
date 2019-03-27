using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data
{
    public sealed class Int16List : FixedList<int>
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        public Int16List(IMemory memory, int offset, int count)
        {
            _memory = memory;
            _offset = offset;
            Count = count;
        }

        public override int Count { get; }

        protected override int GetItem(int index)
        {
            return _memory.Read16(_offset + (index << 1));
        }

        protected override void SetItem(int index, int value)
        {
            _memory.Write16(_offset + (index << 1), value);
        }
    }
}