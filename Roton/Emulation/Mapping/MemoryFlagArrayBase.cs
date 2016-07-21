using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryFlagArrayBase : FixedList<string>, IFlagList
    {
        protected MemoryFlagArrayBase(IMemory memory, int offset)
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
                this[i] = string.Empty;
            }
        }

        private IMemory Memory { get; }

        private int Offset { get; }
    }
}