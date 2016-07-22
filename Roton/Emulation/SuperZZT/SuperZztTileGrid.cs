using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztTileGrid : TileGrid
    {
        public SuperZztTileGrid(IMemory memory)
            : base(memory, 0x2BEB, 96, 80)
        {
        }
    }
}