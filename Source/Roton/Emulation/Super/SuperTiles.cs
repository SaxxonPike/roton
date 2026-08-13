using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperTiles : Tiles
{
    public SuperTiles(IMemory memory, IElementList elementList)
        : base(memory, elementList, 0x2BEB, 96, 80)
    {
    }
}