using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal class MemoryFlagArrayBase : FixedList<string>
    {
        public MemoryFlagArrayBase(Memory memory, int offset)
        {
            this.Memory = memory;
            this.Offset = offset;
        }

        public override string this[int index]
        {
            get
            {
                return Memory.ReadString(Offset + (index * 21));
            }
            set
            {
                Memory.WriteString(Offset + (index * 21), value);
            }
        }

        public override void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i] = "";
            }
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
