using Roton.Core;
using Roton.Core.Collections;

namespace Roton.Emulation.Mapping
{
    internal sealed class ByteString : FixedList<int>
    {
        public ByteString(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        protected override int GetItem(int index)
        {
            return Memory.Read8(Offset + index + 1);
        }

        protected override void SetItem(int index, int value)
        {
            Memory.Write8(Offset + index + 1, value);
        }

        public override int Count => Memory.Read8(Offset);

        private IMemory Memory { get; }

        private int Offset { get; }
    }
}