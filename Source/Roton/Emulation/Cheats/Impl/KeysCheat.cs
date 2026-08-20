using System;
using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "KEYS")]
[Context(Context.Super, "KEYS")]
public sealed class KeysCheat(IEngineAccessor engine) : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        for (var i = 0; i < 7; i++)
            Engine.World.Keys[i] = true;
    }
}