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

        public override int this[int index]
        {
            get { return Memory.Read16(Offset + (index << 1)); }
            set { Memory.Write16(Offset + (index << 1), value); }
        }

        public override int Count { get; }

        public Memory Memory { get; }

        public int Offset { get; }
    }
}