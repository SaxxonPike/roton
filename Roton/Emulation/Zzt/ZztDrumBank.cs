using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.ZZT
{
    public class ZztDrumBank : MemoryDrumBank
    {
        public ZztDrumBank(IMemory memory) : base(memory, 0x7FA4)
        {
        }
    }
}
