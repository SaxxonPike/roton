using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalDrumBank : MemoryDrumBank
    {
        public OriginalDrumBank(IMemory memory) : base(memory, 0x7FA4)
        {
        }
    }
}
