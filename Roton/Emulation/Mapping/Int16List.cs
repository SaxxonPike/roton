using Roton.Core;
using Roton.Core.Collections;
using Roton.Extensions;

namespace Roton.Emulation.Mapping
{
    internal sealed class Int16List : FixedList<int>
    {
        public Int16List(IMemory memory, int offset, int count)
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

        private IMemory Memory { get; }

        private int Offset { get; }
    }
}