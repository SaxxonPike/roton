using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Zzt
{
    public sealed class ZztTiles : Tiles
    {
        public ZztTiles(IMemory memory, IElements elements)
            : base(memory, elements, 0x24B9, 60, 25)
        {
        }
    }
}