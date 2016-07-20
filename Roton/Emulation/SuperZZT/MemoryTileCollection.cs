using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class MemoryTileCollection : MemoryTileCollectionBase
    {
        public MemoryTileCollection(IMemory memory)
            : base(memory, 0x2BEB, 96, 80)
        {
        }
    }
}