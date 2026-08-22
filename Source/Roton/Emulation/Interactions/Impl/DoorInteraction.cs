using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x09)]
[Context(Context.Super, 0x09)]
public sealed class DoorInteraction(
    IEngineAccessor engine,
    IWorld world,
    ITiles tiles,
    IAlerts alerts,
    IFacts facts,
    ISounds sounds,
    IHud hud)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = (tiles[location].Color & 0x70) >> 4;
        var keyIndex = color - 1;
        if (!world.Keys[keyIndex])
        {
            Engine.SetMessage(facts.LongMessageDuration, alerts.DoorLockedMessage(color));
            Engine.PlaySound(3, sounds.DoorLocked);
        }
        else
        {
            world.Keys[keyIndex] = false;
            Engine.RemoveItem(location);
            hud.UpdateStatus();
            Engine.SetMessage(facts.LongMessageDuration, alerts.DoorOpenMessage(color));
            Engine.PlaySound(3, sounds.DoorOpen);
        }
    }
}