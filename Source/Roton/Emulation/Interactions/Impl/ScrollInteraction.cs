using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
public sealed class ScrollInteraction(Lazy<IEngine> engine) : IInteraction
{
    private IEngine Engine => engine.Value;

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        var scrollIndex = Engine.ActorIndexAt(location);
        var actor = Engine.Actors[scrollIndex];

        Engine.PlaySound(2, Engine.MusicEncoder.Encode("c-c+d-d+e-e+f-f+g-g"));
        Engine.ExecuteCode(scrollIndex, actor, "Scroll");
        Engine.RemoveActor(scrollIndex);
    }
}