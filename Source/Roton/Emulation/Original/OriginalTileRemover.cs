using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalTileRemover(
    ITiles tiles,
    IElementList elements,
    IBoardUpdater boardUpdater,
    IDeferred<IAttacker> attacker,
    IPlotter plotter)
    : ITileRemover
{
    public void RemoveActor(Location location, int index, Tile tile)
    {
        attacker.Instance.Harm(index);
        plotter.Plot(location, tile);
    }

    public void RemoveItem(Location location)
    {
        tiles[location].Id = elements.EmptyId;
        boardUpdater.UpdateBoard(location);
    }
}