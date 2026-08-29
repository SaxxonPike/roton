using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
internal sealed class ScrollInteraction(
    IEngineAccessor engine,
    IActorList actorList,
    IMusicEncoder musicEncoder,
    ISoundUnit soundUnit)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var scrollIndex = actorList.ActorIndexAt(location);
        var actor = actorList[scrollIndex];

        soundUnit.PlaySound(2, musicEncoder.Encode("c-c+d-d+e-e+f-f+g-g"));
        Engine.ExecuteCode(scrollIndex, ref actor.Instruction, "Scroll");
        Engine.RemoveActor(scrollIndex);
    }
}