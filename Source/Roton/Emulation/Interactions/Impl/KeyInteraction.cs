using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x08)]
[Context(Context.Super, 0x08)]
public sealed class KeyInteraction(
    IEngineAccessor engine,
    ITiles tiles,
    IWorld world,
    IHud hud,
    IFacts facts,
    IAlerts alerts,
    ISounds sounds)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = tiles[location].Color & 0x07;
        var keyIndex = color - 1;
        if (world.Keys[keyIndex])
        {
            Engine.SetMessage(facts.LongMessageDuration, alerts.KeyAlreadyMessage(color));
            Engine.PlaySound(2, sounds.KeyAlready);
        }
        else
        {
            world.Keys[keyIndex] = true;
            Engine.RemoveItem(location);
            hud.UpdateStatus();
            Engine.SetMessage(facts.LongMessageDuration, alerts.KeyPickupMessage(color));
            Engine.PlaySound(2, sounds.Key);
        }
    }
}