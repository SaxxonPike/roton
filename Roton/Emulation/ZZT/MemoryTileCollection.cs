using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal class MemoryTileCollection : MemoryTileCollectionBase
    {
        public MemoryTileCollection(Memory memory)
            : base(memory, 0x24B9, 60, 25)
        {
        }
    }
}
