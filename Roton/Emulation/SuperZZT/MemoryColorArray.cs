using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryColorArray : MemoryColorArrayBase
    {
        public MemoryColorArray(Memory memory)
            : base(memory, 0x21E7)
        {
        }

        public override int Count => 7;
    }
}