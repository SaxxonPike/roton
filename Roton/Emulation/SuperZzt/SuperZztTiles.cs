using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztTiles : Tiles
    {
        public SuperZztTiles(IMemory memory, IElementList elementList)
            : base(memory, elementList, 0x2BEB, 96, 80)
        {
        }
    }
}