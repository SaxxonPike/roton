using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal class MemoryColorArray : MemoryColorArrayBase
    {
        public MemoryColorArray(Memory memory)
            : base(memory, 0xFFF9)
        {
        }

        public override int Count
        {
            get { return 7; }
        }
    }
}
