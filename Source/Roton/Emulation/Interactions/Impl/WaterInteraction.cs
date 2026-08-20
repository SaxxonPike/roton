using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x13)]
[Context(Context.Super, 0x13)]
public sealed class WaterInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        if (engine.Instance.Config.NoPesterMode)
            return;

        Engine.PlaySound(3, Engine.Sounds.Water);
        Engine.SetMessage(Engine.Facts.ShortMessageDuration, Engine.Alerts.WaterMessage);
    }
}