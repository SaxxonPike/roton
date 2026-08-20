using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x05)]
[Context(Context.Super, 0x05)]
public sealed class AmmoInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        Engine.World.Ammo += Engine.Facts.AmmoPerPickup;
        Engine.RemoveItem(location);
        Engine.UpdateStatus();
        Engine.PlaySound(2, Engine.Sounds.Ammo);
            
        if (!Engine.Alerts.AmmoPickup) 
            return;
            
        Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.AmmoMessage);
        Engine.Alerts.AmmoPickup = false;
    }
}