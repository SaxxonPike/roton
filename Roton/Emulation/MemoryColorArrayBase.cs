namespace Roton.Emulation
{
    internal abstract class MemoryColorArrayBase : FixedList<string>
    {
        public MemoryColorArrayBase(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override string this[int index]
        {
            get { return Memory.ReadString(Offset + index*9); }
            set { Memory.WriteString(Offset + index*9, value); }
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