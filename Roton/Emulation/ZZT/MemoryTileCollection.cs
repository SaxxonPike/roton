using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    internal sealed class MemoryTileCollection : MemoryTileCollectionBase
    {
        public MemoryTileCollection(IMemory memory)
            : base(memory, 0x24B9, 60, 25)
        {
        }
    }
}