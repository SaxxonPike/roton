namespace Roton.Emulation.Data.Impl
{
    public sealed class ByteString : FixedList<int>
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        internal ByteString(IMemory memory, int offset)
        {
            _memory = memory;
            _offset = offset;
        }

        public override int Count => _memory.Read8(_offset);

        protected override int GetItem(int index)
        {
            return _memory.Read8(_offset + index + 1);
        }

        protected override void SetItem(int index, int value)
        {
            _memory.Write8(_offset + index + 1, value);
        }
    }
}