using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryFlagArray : MemoryFlagArrayBase
    {
        public MemoryFlagArray(IMemory memory)
            : base(memory, 0x7863 + 21)
        {
        }

        public override int Count => 16;
    }
}