using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztFlags : Flags
    {
        public SuperZztFlags(IMemory memory)
            : base(memory, 0x7863 + 21)
        {
        }

        public override int Count => 16;
    }
}