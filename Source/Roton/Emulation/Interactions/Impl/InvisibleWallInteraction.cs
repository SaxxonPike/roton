using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1C)]
[Context(Context.Super, 0x1C)]
public sealed class InvisibleWallInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        tiles[location].Id = Engine.Elements.NormalId;
        Engine.UpdateBoard(location);
        Engine.PlaySound(3, Engine.Sounds.Invisible);
        Engine.SetMessage(Engine.Facts.ShortMessageDuration, Engine.Alerts.InvisibleMessage);
    }
}