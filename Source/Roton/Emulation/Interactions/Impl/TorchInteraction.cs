using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, id: 0x06)]
internal sealed class TorchInteraction(
    ISounds sounds,
    IWorld world,
    IHud hud,
    IAlerts alerts,
    IFacts facts,
    ISoundPlayer soundPlayer,
    IMessenger messenger,
    ITileRemover tileRemover)
    : IInteraction
{
    /// <remarks>
    /// RoZ: ElementTorchTouch
    /// </remarks>
    public void Interact(Location location, int index, ref Vector vector)
    {
        world.Torches++;
        tileRemover.RemoveItem(location);
        hud.UpdateStatus();
        if (alerts.TorchPickup)
        {
            messenger.SetMessage(facts.LongMessageDuration, alerts.TorchMessage);
            alerts.TorchPickup = false;
        }

        soundPlayer.PlaySound(3, sounds.Torch);
    }
}