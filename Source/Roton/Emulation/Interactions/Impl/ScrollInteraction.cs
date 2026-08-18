using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
public sealed class ScrollInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var scrollIndex = Engine.ActorIndexAt(location);
        var actor = Engine.Actors[scrollIndex];

        Engine.PlaySound(2, Engine.MusicEncoder.Encode("c-c+d-d+e-e+f-f+g-g"));
        Engine.ExecuteCode(scrollIndex, ref actor.Instruction, "Scroll");
        Engine.RemoveActor(scrollIndex);
    }
}