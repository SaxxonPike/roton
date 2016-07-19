namespace Roton.Emulation
{
    internal abstract class MemoryFlagArrayBase : FixedList<string>
    {
        public MemoryFlagArrayBase(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override string this[int index]
        {
            get { return Memory.ReadString(Offset + index*21); }
            set { Memory.WriteString(Offset + index*21, value); }
        }

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i] = "";
            }
        }

        public Memory Memory { get; }

        public int Offset { get; }
    }
}