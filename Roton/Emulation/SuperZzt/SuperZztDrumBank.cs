using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztDrumBank : MemoryDrumBank
    {
        public SuperZztDrumBank(IMemory memory) : base(memory, 0xD0B2)
        {
        }
    }
}
