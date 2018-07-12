using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperTiles : Tiles
    {
        public SuperTiles(IMemory memory, IElementList elementList)
            : base(memory, elementList, 0x2BEB, 96, 80)
        {
        }
    }
}