using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztDrumBank : MemoryDrumBank
    {
        public SuperZztDrumBank(IMemory memory) : base(memory, 0xD0B2)
        {
        }
    }
}
