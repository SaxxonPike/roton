using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1C)]
[Context(Context.Super, 0x1C)]
public sealed class InvisibleWallInteraction(Lazy<IEngine> engine) : IInteraction
{
    private IEngine Engine => engine.Value;

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        Engine.Tiles[location].Id = Engine.ElementList.NormalId;
        Engine.UpdateBoard(location);
        Engine.PlaySound(3, Engine.Sounds.Invisible);
        Engine.SetMessage(Engine.Facts.ShortMessageDuration, Engine.Alerts.InvisibleMessage);
    }
}