using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperDrumBank : MemoryDrumBank
    {
        public SuperDrumBank(IMemory memory) : base(memory, 0xD0B2)
        {
        }
    }
}
