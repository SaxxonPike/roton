using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztFlags : Flags
    {
        public SuperZztFlags(IMemory memory)
            : base(memory, 0x7863 + 21)
        {
        }

        public override int Count => 16;
    }
}