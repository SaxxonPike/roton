using System;
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
public sealed class PuzzleInteraction(Lazy<IEngine> engine) : IInteraction
{
    private IEngine Engine => engine.Value;

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        Engine.Push(location, vector);
        Engine.PlaySound(2, Engine.Sounds.Push);
    }
}