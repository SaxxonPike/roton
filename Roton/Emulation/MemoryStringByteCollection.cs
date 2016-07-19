namespace Roton.Emulation
{
    internal sealed class MemoryStringByteCollection : FixedList<int>
    {
        public MemoryStringByteCollection(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override int this[int index]
        {
            get { return Memory.Read8(Offset + index + 1); }
            set { Memory.Write8(Offset + index + 1, value); }
        }

        public override int Count => Memory.Read8(Offset);

        public Memory Memory { get; }

        public int Offset { get; }
    }
}