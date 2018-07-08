using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztDrumBank : MemoryDrumBank
    {
        public SuperZztDrumBank(IMemory memory) : base(memory, 0xD0B2)
        {
        }
    }
}
