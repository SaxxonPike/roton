using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperActorRemover(
    IPlotter plotter)
    : IActorRemover
{
    public void RemoveActor(Location location, int index, Tile tile) => 
        plotter.Plot(location, tile);
}