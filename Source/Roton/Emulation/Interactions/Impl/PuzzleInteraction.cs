using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x18)]
[Context(Context.Original, 0x19)]
[Context(Context.Original, 0x1A)]
[Context(Context.Super, 0x18)]
[Context(Context.Super, 0x19)]
[Context(Context.Super, 0x1A)]
internal sealed class PuzzleInteraction(
    ISounds sounds,
    ISoundUnit soundUnit,
    IPusher pusher)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        pusher.Push(location, vector);
        soundUnit.PlaySound(2, sounds.Push);
    }
}