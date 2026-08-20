using System;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "GEMS")]
[Context(Context.Super, "GEMS")]
public sealed class GemsCheat(IEngineAccessor engine) : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        Engine.World.Gems += 5;
    }
}