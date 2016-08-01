using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    internal class SuperZztDrumBank : MemoryDrumBank
    {
        public SuperZztDrumBank(IMemory memory) : base(memory, 0xD0B2)
        {
        }
    }
}
