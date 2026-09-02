using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalTileRemover(
    ITiles tiles,
    IElementList elements,
    IBoardUpdater boardUpdater,
    IEngineAccessor engine)
    : ITileRemover
{
    private IEngine Engine => engine.Instance;
    
    public void RemoveActor(Location location, int index, Tile tile)
    {
        Engine.Harm(index);
        Engine.PlotTile(location, tile);
    }

    public void RemoveItem(Location location)
    {
        tiles[location].Id = elements.EmptyId;
        boardUpdater.UpdateBoard(location);
    }
}