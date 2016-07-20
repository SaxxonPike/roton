using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryColorArrayBase : FixedList<string>
    {
        public MemoryColorArrayBase(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        protected override string GetItem(int index)
        {
            return Memory.ReadString(Offset + index * 9);
        }

        protected override void SetItem(int index, string value)
        {
            Memory.WriteString(Offset + index * 9, value);
        }

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i] = "";
            }
        }

        public IMemory Memory { get; }

        public int Offset { get; }
    }
}