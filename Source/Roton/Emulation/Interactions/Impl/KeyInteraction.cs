using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x08)]
[Context(Context.Super, 0x08)]
internal sealed class KeyInteraction(
    ITiles tiles,
    IWorld world,
    IHud hud,
    IFacts facts,
    IAlerts alerts,
    ISounds sounds,
    ISoundUnit soundUnit,
    IMessenger messenger,
    ITileRemover tileRemover)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = tiles[location].Color & 0x07;
        var keyIndex = color - 1;
        if (world.Keys[keyIndex])
        {
            messenger.SetMessage(facts.LongMessageDuration, alerts.KeyAlreadyMessage(color));
            soundUnit.PlaySound(2, sounds.KeyAlready);
        }
        else
        {
            world.Keys[keyIndex] = true;
            tileRemover.RemoveItem(location);
            hud.UpdateStatus();
            messenger.SetMessage(facts.LongMessageDuration, alerts.KeyPickupMessage(color));
            soundUnit.PlaySound(2, sounds.Key);
        }
    }
}