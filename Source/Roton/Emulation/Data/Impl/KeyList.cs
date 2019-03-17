namespace Roton.Emulation.Data.Impl
{
    public abstract class KeyList : FixedList<bool>, IKeyList
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        protected KeyList(IMemory memory, int offset)
        {
            _memory = memory;
            _offset = offset;
        }

        public override int Count => 7;

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
                this[i] = false;
        }

        protected override bool GetItem(int index) 
            => _memory.ReadBool(_offset + index);

        protected override void SetItem(int index, bool value) 
            => _memory.WriteBool(_offset + index, value);
    }
}