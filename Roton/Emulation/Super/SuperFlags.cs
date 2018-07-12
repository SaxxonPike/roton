using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperFlags : Flags
    {
        public SuperFlags(IMemory memory)
            : base(memory, 0x7863 + 21)
        {
        }

        public override int Count => 16;
    }
}