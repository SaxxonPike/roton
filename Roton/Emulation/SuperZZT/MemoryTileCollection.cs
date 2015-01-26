using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class MemoryTileCollection : MemoryTileCollectionBase
    {
        public MemoryTileCollection(Memory memory)
            : base(memory, 0x2BEB, 96, 80)
        {
        }
    }
}
