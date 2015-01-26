using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal abstract class MemoryBoardBase : Board
    {
        public MemoryBoardBase(Memory memory)
            : base()
        {
            this.Memory = memory;
        }

        public Memory Memory
        {
            get;
            private set;
        }
    }
}
