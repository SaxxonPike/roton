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
public sealed class PuzzleInteraction : IInteraction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public PuzzleInteraction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        Engine.Push(location, vector);
        Engine.PlaySound(2, Engine.Sounds.Push);
    }
}