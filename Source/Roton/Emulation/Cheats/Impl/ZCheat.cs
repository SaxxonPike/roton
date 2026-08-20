using System;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Super, "Z")]
public sealed class ZCheat(IEngineAccessor engine) : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        Engine.World.Stones++;
    }
}