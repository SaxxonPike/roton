using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class MemoryStringByteCollection : FixedList<int>
    {
        public MemoryStringByteCollection(Memory memory, int offset)
        {
            this.Memory = memory;
            this.Offset = offset;
        }

        public override int this[int index]
        {
            get
            {
                return Memory.Read8(Offset + index + 1);
            }
            set
            {
                Memory.Write8(Offset + index + 1, value);
            }
        }

        public override int Count
        {
            get { return Memory.Read8(Offset); }
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
