using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1B)]
[Context(Context.Super, 0x1B)]
public sealed class FakeWallInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        if (!Engine.Alerts.FakeWall) return;

        Engine.Alerts.FakeWall = false;
        Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.FakeMessage);
    }
}