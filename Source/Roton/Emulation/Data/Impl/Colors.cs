namespace Roton.Emulation.Data.Impl
{
    public abstract class Colors : FixedList<string>, IColors
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        protected Colors(IMemory memory, int offset)
        {
            _memory = memory;
            _offset = offset;
        }

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i] = string.Empty;
            }
        }

        protected override string GetItem(int index)
        {
            return _memory.ReadString(_offset + index * 9);
        }

        protected override void SetItem(int index, string value)
        {
            _memory.WriteString(_offset + index * 9, value);
        }
    }
}