using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x05)]
[Context(Context.Super, 0x05)]
internal sealed class AmmoInteraction(
    IWorld world,
    ISounds sounds,
    IAlerts alerts,
    IFacts facts,
    ISoundUnit soundUnit,
    IHud hud,
    IMessenger messenger,
    ITileRemover tileRemover)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        world.Ammo += facts.AmmoPerPickup;
        tileRemover.RemoveItem(location);
        hud.UpdateStatus();
        soundUnit.PlaySound(2, sounds.Ammo);

        if (!alerts.AmmoPickup)
            return;

        messenger.SetMessage(facts.LongMessageDuration, alerts.AmmoMessage);
        alerts.AmmoPickup = false;
    }
}