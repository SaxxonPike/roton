using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztColors : Colors
    {
        public SuperZztColors(IMemory memory)
            : base(memory, 0x21E7)
        {
        }

        public override int Count => 7;
    }
}