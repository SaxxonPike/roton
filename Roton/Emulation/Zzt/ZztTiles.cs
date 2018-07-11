using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Zzt
{
    [ContextEngine(ContextEngine.Zzt)]
    public sealed class ZztTiles : Tiles
    {
        public ZztTiles(IMemory memory, IElementList elementList)
            : base(memory, elementList, 0x24B9, 60, 25)
        {
        }
    }
}