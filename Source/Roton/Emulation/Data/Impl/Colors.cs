namespace Roton.Emulation.Data.Impl
{
    public abstract class Colors : FixedList<string>, IColors
    {
        protected Colors(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        private IMemory Memory { get; }

        private int Offset { get; }

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i] = string.Empty;
            }
        }

        protected override string GetItem(int index)
        {
            return Memory.ReadString(Offset + index*9);
        }

        protected override void SetItem(int index, string value)
        {
            Memory.WriteString(Offset + index*9, value);
        }
    }
}