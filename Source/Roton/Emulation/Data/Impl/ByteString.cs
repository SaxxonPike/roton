namespace Roton.Emulation.Data.Impl
{
    public sealed class ByteString : FixedList<int>
    {
        internal ByteString(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override int Count => Memory.Read8(Offset);

        private IMemory Memory { get; }

        private int Offset { get; }

        protected override int GetItem(int index)
        {
            return Memory.Read8(Offset + index + 1);
        }

        protected override void SetItem(int index, int value)
        {
            Memory.Write8(Offset + index + 1, value);
        }
    }
}