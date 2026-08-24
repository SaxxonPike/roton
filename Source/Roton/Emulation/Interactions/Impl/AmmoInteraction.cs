using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x05)]
[Context(Context.Super, 0x05)]
public sealed class AmmoInteraction(
    IEngineAccessor engine,
    IWorld world,
    ISounds sounds,
    IAlerts alerts,
    IFacts facts,
    ISoundUnit soundUnit)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        world.Ammo += facts.AmmoPerPickup;
        Engine.RemoveItem(location);
        Engine.UpdateStatus();
        soundUnit.PlaySound(2, sounds.Ammo);

        if (!alerts.AmmoPickup)
            return;

        Engine.SetMessage(facts.LongMessageDuration, alerts.AmmoMessage);
        alerts.AmmoPickup = false;
    }
}