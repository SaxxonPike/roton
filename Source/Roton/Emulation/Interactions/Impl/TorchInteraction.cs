using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x06)]
internal sealed class TorchInteraction(
    ISounds sounds,
    IWorld world,
    IHud hud,
    IAlerts alerts,
    IFacts facts,
    ISoundUnit soundUnit,
    IFeatures features,
    IMessenger messenger)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        world.Torches++;
        features.RemoveItem(location);
        hud.UpdateStatus();
        if (alerts.TorchPickup)
        {
            messenger.SetMessage(facts.LongMessageDuration, alerts.TorchMessage);
            alerts.TorchPickup = false;
        }

        soundUnit.PlaySound(3, sounds.Torch);
    }
}