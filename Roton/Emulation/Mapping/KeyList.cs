using Roton.Core;
using Roton.Core.Collections;
using Roton.Extensions;

namespace Roton.Emulation.Mapping
{
    internal sealed class KeyList : FixedList<bool>, IKeyList
    {
        public KeyList(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override int Count => 7;

        private IMemory Memory { get; }

        private int Offset { get; }

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i] = false;
            }
        }

        protected override bool GetItem(int index)
        {
            return Memory.ReadBool(Offset + index);
        }

        protected override void SetItem(int index, bool value)
        {
            Memory.WriteBool(Offset + index, value);
        }
    }
}