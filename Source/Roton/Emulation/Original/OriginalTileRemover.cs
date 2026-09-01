using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public class OriginalTileRemover(
    ITiles tiles,
    IElementList elements,
    IBoardUpdater boardUpdater)
    : ITileRemover
{
    public void RemoveItem(Location location)
    {
        tiles[location].Id = elements.EmptyId;
        boardUpdater.UpdateBoard(location);
    }
}