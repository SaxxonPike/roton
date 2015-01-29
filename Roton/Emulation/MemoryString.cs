using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class MemoryString
    {
        public static implicit operator string(MemoryString source)
        {
            return source.ToString();
        }

        public MemoryString(Memory memory, int offset)
        {
            this.Memory = memory;
            this.Offset = offset;
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

        public override string ToString()
        {
            return Memory.ReadString(Offset);
        }
    }
}
