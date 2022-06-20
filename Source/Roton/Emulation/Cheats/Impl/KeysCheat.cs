using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "KEYS")]
[Context(Context.Super, "KEYS")]
public sealed class KeysCheat : ICheat
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public KeysCheat(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Execute(string name, bool clear)
    {
        for (var i = 0; i < 7; i++)
            Engine.World.Keys[i] = true;
    }
}