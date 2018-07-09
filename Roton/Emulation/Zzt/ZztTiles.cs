using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Zzt
{
    public sealed class ZztTiles : Tiles
    {
        public ZztTiles(IMemory memory, IElementList elementList)
            : base(memory, elementList, 0x24B9, 60, 25)
        {
        }
    }
}