using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x08)]
[Context(Context.Super, 0x08)]
public sealed class KeyInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = Engine.Tiles[location].Color & 0x07;
        var keyIndex = color - 1;
        if (Engine.World.Keys[keyIndex])
        {
            Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.KeyAlreadyMessage(color));
            Engine.PlaySound(2, Engine.Sounds.KeyAlready);
        }
        else
        {
            Engine.World.Keys[keyIndex] = true;
            Engine.RemoveItem(location);
            Engine.Hud.UpdateStatus();
            Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.KeyPickupMessage(color));
            Engine.PlaySound(2, Engine.Sounds.Key);
        }
    }
}