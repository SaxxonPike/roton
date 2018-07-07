using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztFlags : Flags
    {
        public ZztFlags(IMemory memory)
            : base(memory, 0x4837 + 21)
        {
        }

        public override int Count => 10;
    }
}