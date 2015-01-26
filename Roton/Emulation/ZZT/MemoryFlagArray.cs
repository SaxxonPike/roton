using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal class MemoryFlagArray : MemoryFlagArrayBase
    {
        public MemoryFlagArray(Memory memory)
            : base(memory, 0x4837 + 21)
        {
        }

        public override int Count
        {
            get { return 10; }
        }
    }
}
