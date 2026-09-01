using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperForestHandler(
    ITiles tiles,
    IElementList elements)
    : IForestHandler
{
    public void ClearForest(Location location) =>
        tiles[location] = new Tile(elements.FloorId, 0x02);
}