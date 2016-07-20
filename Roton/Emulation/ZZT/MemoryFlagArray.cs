using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    internal sealed class MemoryFlagArray : MemoryFlagArrayBase
    {
        public MemoryFlagArray(IMemory memory)
            : base(memory, 0x4837 + 21)
        {
        }

        public override int Count => 10;
    }
}