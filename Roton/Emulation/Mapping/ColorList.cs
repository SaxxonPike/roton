using Roton.Core;
using Roton.Core.Collections;
using Roton.Extensions;

namespace Roton.Emulation.Mapping
{
    internal abstract class ColorList : FixedList<string>, IColorList
    {
        protected ColorList(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        protected override string GetItem(int index)
        {
            return Memory.ReadString(Offset + index*9);
        }

        protected override void SetItem(int index, string value)
        {
            Memory.WriteString(Offset + index*9, value);
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