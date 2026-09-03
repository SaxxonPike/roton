using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x09)]
[Context(Context.Super, 0x09)]
internal sealed class DoorInteraction(
    IWorld world,
    ITiles tiles,
    IAlerts alerts,
    IFacts facts,
    ISounds sounds,
    IHud hud,
    ISoundPlayer soundPlayer,
    IMessenger messenger,
    ITileRemover tileRemover)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = (tiles[location].Color & 0x70) >> 4;
        var keyIndex = color - 1;
        if (!world.Keys[keyIndex])
        {
            messenger.SetMessage(facts.LongMessageDuration, alerts.DoorLockedMessage(color));
            soundPlayer.PlaySound(3, sounds.DoorLocked);
        }
        else
        {
            world.Keys[keyIndex] = false;
            tileRemover.RemoveItem(location);
            hud.UpdateStatus();
            messenger.SetMessage(facts.LongMessageDuration, alerts.DoorOpenMessage(color));
            soundPlayer.PlaySound(3, sounds.DoorOpen);
        }
    }
}