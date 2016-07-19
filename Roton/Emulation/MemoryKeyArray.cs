namespace Roton.Emulation
{
    internal sealed class MemoryKeyArray : FixedList<bool>
    {
        public MemoryKeyArray(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        protected override bool GetItem(int index)
        {
            return Memory.ReadBool(Offset + index);
        }

        protected override void SetItem(int index, bool value)
        {
            Memory.WriteBool(Offset + index, value);
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