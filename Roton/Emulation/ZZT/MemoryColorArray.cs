using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    internal sealed class MemoryColorArray : MemoryColorArrayBase
    {
        public MemoryColorArray(IMemory memory)
            : base(memory, 0xFFF9)
        {
        }

        public override int Count => 7;
    }
}