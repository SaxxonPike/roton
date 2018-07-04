using Roton.Core;
using Roton.Emulation.Mapping;

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