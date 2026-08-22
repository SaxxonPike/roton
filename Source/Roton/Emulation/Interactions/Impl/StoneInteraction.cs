using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Super, 0x40)]
public sealed class StoneInteraction(
    IEngineAccessor engine,
    IWorld world,
    IHud hud,
    IFacts facts,
    IAlerts alerts) 
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        if (world.Stones < 0)
            world.Stones = 0;

        world.Stones++;
        Engine.Destroy(location);
        hud.UpdateStatus();
        Engine.SetMessage(facts.LongMessageDuration, alerts.StoneMessage);
    }
}