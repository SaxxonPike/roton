using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryColorArray : MemoryColorArrayBase
    {
        public MemoryColorArray(IMemory memory)
            : base(memory, 0x21E7)
        {
        }

        public override int Count => 7;
    }
}