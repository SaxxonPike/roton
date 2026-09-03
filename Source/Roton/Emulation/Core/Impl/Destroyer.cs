using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Destroyer(
    IActorList actors,
    ITileRemover tileRemover,
    IDeferred<IDamager> damager)
    : IDestroyer
{
    public void Destroy(Location location)
    {
        var index = actors.ActorIndexAt(location);
        if (index == -1)
            tileRemover.RemoveItem(location);
        else
            damager.Instance.Harm(index);
    }
}