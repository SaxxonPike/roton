using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0E)]
[Context(Context.Super, 0x0E)]
internal sealed class EnergizerInteraction(
    ISounds sounds,
    IWorld world,
    IHud hud,
    IFacts facts,
    IAlerts alerts,
    ISoundPlayer soundPlayer,
    IBroadcaster broadcaster,
    IMessenger messenger,
    ITileRemover tileRemover)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        soundPlayer.PlaySound(9, sounds.Energizer);
        tileRemover.RemoveItem(location);
        world.EnergyCycles = facts.EnergyCyclesPerEnergizer;
        hud.UpdateStatus();

        if (alerts.EnergizerPickup)
        {
            alerts.EnergizerPickup = false;
            messenger.SetMessage(facts.LongMessageDuration, alerts.EnergizerMessage);
        }

        broadcaster.BroadcastLabel(0, facts.EnergizeLabel, false);
    }
}