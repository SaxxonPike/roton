using Roton.Core;
using Roton.Core.Collections;
using Roton.Extensions;

namespace Roton.Emulation.Mapping
{
    internal abstract class FlagList : FixedList<string>, IFlagList
    {
        protected FlagList(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        protected override string GetItem(int index)
        {
            return Memory.ReadString(Offset + index*21);
        }

        protected override void SetItem(int index, string value)
        {
            Memory.WriteString(Offset + index*21, value);
        }

        public override void Add(string item)
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(GetItem(i)))
                    continue;

                SetItem(i, item);
                return;
            }
        }

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i] = string.Empty;
            }
        }

        public override bool Contains(string item)
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                if (GetItem(i) == item)
                    return true;
            }
            return false;
        }

        public override bool Remove(string item)
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                if (GetItem(i) != item)
                    continue;

                SetItem(i, string.Empty);
                return true;
            }
            return false;
        }

        private IMemory Memory { get; }

        private int Offset { get; }
    }
}