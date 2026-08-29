using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0E)]
[Context(Context.Super, 0x0E)]
internal sealed class EnergizerInteraction(
    IEngineAccessor engine,
    ISounds sounds,
    IWorld world,
    IHud hud,
    IFacts facts,
    IAlerts alerts,
    ISoundUnit soundUnit,
    IFeatures features,
    IBroadcaster broadcaster)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        soundUnit.PlaySound(9, sounds.Energizer);
        features.RemoveItem(location);
        world.EnergyCycles = facts.EnergyCyclesPerEnergizer;
        hud.UpdateStatus();

        if (alerts.EnergizerPickup)
        {
            alerts.EnergizerPickup = false;
            Engine.SetMessage(facts.LongMessageDuration, alerts.EnergizerMessage);
        }

        broadcaster.BroadcastLabel(0, facts.EnergizeLabel, false);
    }
}