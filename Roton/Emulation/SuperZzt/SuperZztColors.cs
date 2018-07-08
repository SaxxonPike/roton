using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztColors : Colors
    {
        public SuperZztColors(IMemory memory)
            : base(memory, 0x21E7)
        {
        }

        public override int Count => 7;
    }
}