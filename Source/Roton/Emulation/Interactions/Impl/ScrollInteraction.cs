using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
internal sealed class ScrollInteraction(
    IEngineAccessor engine,
    IActorList actors,
    IMusicEncoder musicEncoder,
    ISoundUnit soundUnit)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    private readonly byte[] _scrollMusic = EncodeScrollMusic(musicEncoder);

    /// <summary>
    /// Pre caches the scroll music.
    /// </summary>
    private static byte[] EncodeScrollMusic(IMusicEncoder musicEncoder)
    {
        using var mem = musicEncoder.Encode("c-c+d-d+e-e+f-f+g-g");
        return mem.Span.ToArray();
    }

    public void Interact(Location location, int index, ref Vector vector)
    {
        var scrollIndex = actors.ActorIndexAt(location);
        var actor = actors[scrollIndex];

        soundUnit.PlaySound(2, _scrollMusic);
        Engine.ExecuteCode(scrollIndex, ref actor.Instruction, "Scroll");
        Engine.RemoveActor(scrollIndex);
    }
}