using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class MemoryLocation16 : Location
    {
        public MemoryLocation16(Memory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
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

        public override int X
        {
            get
            {
                return Memory.Read16(Offset + 0x00);
            }
            set
            {
                Memory.Write16(Offset + 0x00, value);
            }
        }

        public override int Y
        {
            get
            {
                return Memory.Read16(Offset + 0x02);
            }
            set
            {
                Memory.Write16(Offset + 0x02, value);
            }
        }
    }
}
