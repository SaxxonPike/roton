using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class MemoryColorArray : MemoryColorArrayBase
    {
        public MemoryColorArray(Memory memory)
            : base(memory, 0x21E7)
        {
        }

        public override int Count
        {
            get { return 7; }
        }
    }
}
