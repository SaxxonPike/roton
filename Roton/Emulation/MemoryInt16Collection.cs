namespace Roton.Emulation
{
    internal sealed class MemoryInt16Collection : FixedList<int>
    {
        public MemoryInt16Collection(Memory memory, int offset, int count)
        {
            Memory = memory;
            Offset = offset;
            Count = count;
        }

        protected override int GetItem(int index)
        {
            return Memory.Read16(Offset + (index << 1));
        }

        protected override void SetItem(int index, int value)
        {
            Memory.Write16(Offset + (index << 1), value);
        }

        public override int Count { get; }

        public Memory Memory { get; }

        public int Offset { get; }
    }
}