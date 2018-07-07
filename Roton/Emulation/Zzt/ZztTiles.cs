using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztTiles : Tiles
    {
        public ZztTiles(IMemory memory, IElements elements)
            : base(memory, elements, 0x24B9, 60, 25)
        {
        }
    }
}