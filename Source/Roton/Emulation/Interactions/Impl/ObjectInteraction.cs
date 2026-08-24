using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
public sealed class ObjectInteraction(
    IEngineAccessor engine,
    IFacts facts,
    IActorList actorList)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var objectIndex = actorList.ActorIndexAt(location);
        Engine.BroadcastLabel(-objectIndex, facts.TouchLabel, false);
    }
}