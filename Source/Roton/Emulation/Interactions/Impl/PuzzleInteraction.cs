using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x18)]
[Context(Context.Original, 0x19)]
[Context(Context.Original, 0x1A)]
[Context(Context.Super, 0x18)]
[Context(Context.Super, 0x19)]
[Context(Context.Super, 0x1A)]
public sealed class PuzzleInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        Engine.Push(location, vector);
        Engine.PlaySound(2, Engine.Sounds.Push);
    }
}