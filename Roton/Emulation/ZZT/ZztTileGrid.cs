using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztTileGrid : TileGrid
    {
        public ZztTileGrid(IMemory memory)
            : base(memory, 0x24B9, 60, 25)
        {
        }
    }
}