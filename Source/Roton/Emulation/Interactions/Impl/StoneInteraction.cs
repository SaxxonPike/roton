using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Super, 0x40)]
public sealed class StoneInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        if (Engine.World.Stones < 0)
            Engine.World.Stones = 0;

        Engine.World.Stones++;
        Engine.Destroy(location);
        Engine.Hud.UpdateStatus();
        Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.StoneMessage);
    }
}