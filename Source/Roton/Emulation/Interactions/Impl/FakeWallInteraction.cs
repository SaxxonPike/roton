using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1B)]
[Context(Context.Super, 0x1B)]
public sealed class FakeWallInteraction(
    IEngineAccessor engine,
    IAlerts alerts,
    IFacts facts)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        if (!alerts.FakeWall) return;

        alerts.FakeWall = false;
        Engine.SetMessage(facts.LongMessageDuration, alerts.FakeMessage);
    }
}