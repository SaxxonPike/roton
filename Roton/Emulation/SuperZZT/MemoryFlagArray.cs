using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class MemoryFlagArray : MemoryFlagArrayBase
    {
        public MemoryFlagArray(Memory memory)
            : base(memory, 0x7863 + 21)
        {
        }

        public override int Count
        {
            get { return 16; }
        }
    }
}
