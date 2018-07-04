using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztColors : Colors
    {
        public ZztColors(IMemory memory)
            : base(memory, 0xFFF9)
        {
        }

        public override int Count => 7;
    }
}