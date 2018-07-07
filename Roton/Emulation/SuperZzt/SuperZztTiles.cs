using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztTiles : Tiles
    {
        public SuperZztTiles(IMemory memory, IElements elements)
            : base(memory, elements, 0x2BEB, 96, 80)
        {
        }
    }
}