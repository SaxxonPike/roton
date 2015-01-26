using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class MemoryInt16Collection : FixedList<int>
    {
        int _count;

        public MemoryInt16Collection(Memory memory, int offset, int count)
        {
            this.Memory = memory;
            this.Offset = offset;
            _count = count;
        }

        public override int this[int index]
        {
            get
            {
                return Memory.Read16(Offset + (index << 1));
            }
            set
            {
                Memory.Write16(Offset + (index << 1), value);
            }
        }

        public override int Count
        {
            get { return _count; }
        }

        public Memory Memory
        {
            get;
            private set;
        }

        public int Offset
        {
            get;
            private set;
        }
    }
}
