using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    internal class ZztDrumBank : MemoryDrumBank
    {
        public ZztDrumBank(IMemory memory) : base(memory, 0x7FA4)
        {
        }
    }
}
