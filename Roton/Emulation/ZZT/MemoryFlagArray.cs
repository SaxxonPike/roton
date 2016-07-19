using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    internal sealed class MemoryFlagArray : MemoryFlagArrayBase
    {
        public MemoryFlagArray(Memory memory)
            : base(memory, 0x4837 + 21)
        {
        }

        public override int Count => 10;
    }
}