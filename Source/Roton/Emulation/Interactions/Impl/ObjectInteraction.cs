using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, id: 0x24)]
[Context(Context.Super, id: 0x24)]
internal sealed class ObjectInteraction(
    IFacts facts,
    IActorList actors,
    IBroadcaster broadcaster)
    : IInteraction
{
    /// <remarks>
    /// RoZ: ElementObjectTouch
    /// </remarks>
    public void Interact(Location location, int index, ref Vector vector)
    {
        var objectIndex = actors.IndexAt(location);
        broadcaster.BroadcastLabel(-objectIndex, facts.TouchLabel, false);
    }
}