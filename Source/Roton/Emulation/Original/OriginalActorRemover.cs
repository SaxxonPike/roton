using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalActorRemover(
    IDeferred<IDamager> damager,
    IPlotter plotter) : IActorRemover
{
    public void RemoveActor(Location location, int index, Tile tile)
    {
        damager.Instance.Harm(index);
        plotter.Plot(location, tile);
    }
}