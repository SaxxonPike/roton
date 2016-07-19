using Roton.Core;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryFlagArrayBase : FixedList<string>
    {
        public MemoryFlagArrayBase(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        protected override string GetItem(int index)
        {
            return Memory.ReadString(Offset + index * 21);
        }

        protected override void SetItem(int index, string value)
        {
            Memory.WriteString(Offset + index * 21, value);
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