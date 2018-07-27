using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperDrumBank : MemoryDrumBank
    {
        public SuperDrumBank(IMemory memory) : base(memory, 0xD0B2)
        {
        }
    }
}
