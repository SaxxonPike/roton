using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
internal sealed class ObjectInteraction(
    IFacts facts,
    IActorList actorList,
    IBroadcaster broadcaster)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        var objectIndex = actorList.ActorIndexAt(location);
        broadcaster.BroadcastLabel(-objectIndex, facts.TouchLabel, false);
    }
}