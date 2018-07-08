using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

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