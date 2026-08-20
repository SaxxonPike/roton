using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x07)]
[Context(Context.Super, 0x07)]
public sealed class GemInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        Engine.World.Health += Engine.Facts.HealthPerGem;
        Engine.World.Gems += 1;
        Engine.World.Score += Engine.Facts.ScorePerGem;
        Engine.RemoveItem(location);
        Engine.Hud.UpdateStatus();
        Engine.PlaySound(2, Engine.Sounds.Gem);

        if (!Engine.Alerts.GemPickup)
            return;

        Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.GemMessage);
        Engine.Alerts.GemPickup = false;
    }
}