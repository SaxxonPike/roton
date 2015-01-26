using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class MemoryKeyArray : FixedList<bool>
    {
        public MemoryKeyArray(Memory memory, int offset)
        {
            this.Memory = memory;
            this.Offset = offset;
        }

        public override bool this[int index]
        {
            get
            {
                return Memory.ReadBool(Offset + index);
            }
            set
            {
                Memory.WriteBool(Offset + index, value);
            }
        }

        public override void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i] = false;
            }
        }

        public override int Count
        {
            get { return 7; }
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
