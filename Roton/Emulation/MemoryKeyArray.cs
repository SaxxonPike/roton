namespace Roton.Emulation
{
    internal sealed class MemoryKeyArray : FixedList<bool>
    {
        public MemoryKeyArray(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override bool this[int index]
        {
            get { return Memory.ReadBool(Offset + index); }
            set { Memory.WriteBool(Offset + index, value); }
        }

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i] = false;
            }
        }

        public override int Count => 7;

        public Memory Memory { get; }

        public int Offset { get; }
    }
}