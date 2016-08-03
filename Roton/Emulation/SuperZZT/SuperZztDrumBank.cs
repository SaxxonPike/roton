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
