using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalTileRemover(
    ITiles tiles,
    IElementList elements,
    IBoardUpdater boardUpdater,
    //IAttacker attacker,
    IPlotter plotter,
    IServiceProvider serviceProvider)
    : ITileRemover
{
    private readonly Lazy<IAttacker> _attacker = new(() =>
        (IAttacker)serviceProvider.GetService(typeof(IAttacker)));
    
    public void RemoveActor(Location location, int index, Tile tile)
    {
        _attacker.Value.Harm(index);
        plotter.Plot(location, tile);
    }

    public void RemoveItem(Location location)
    {
        tiles[location].Id = elements.EmptyId;
        boardUpdater.UpdateBoard(location);
    }
}