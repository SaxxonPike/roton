using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperColors : Colors
    {
        public SuperColors(IMemory memory)
            : base(memory, 0x21E7)
        {
        }

        public override int Count => 7;
    }
}