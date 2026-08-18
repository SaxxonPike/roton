using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x13)]
[Context(Context.Super, 0x13)]
public sealed class WaterInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        Engine.PlaySound(3, Engine.Sounds.Water);
        Engine.SetMessage(Engine.Facts.ShortMessageDuration, Engine.Alerts.WaterMessage);
    }
}