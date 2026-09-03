using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1B)]
[Context(Context.Super, 0x1B)]
internal sealed class FakeWallInteraction(
    IAlerts alerts,
    IFacts facts,
    IMessenger messenger)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        if (!alerts.FakeWall) return;

        alerts.FakeWall = false;
        messenger.SetMessage(facts.LongMessageDuration, alerts.FakeMessage);
    }
}