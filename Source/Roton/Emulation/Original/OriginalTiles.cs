using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalTiles : Tiles
    {
        public OriginalTiles(IMemory memory, IElementList elementList)
            : base(memory, elementList, 0x24B9, 60, 25)
        {
        }
    }
}