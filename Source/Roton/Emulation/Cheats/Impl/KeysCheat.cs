using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "KEYS")]
[Context(Context.Super, "KEYS")]
public sealed class KeysCheat(Lazy<IEngine> engine) : ICheat
{
    private IEngine Engine => engine.Value;

    public void Execute(string name, bool clear)
    {
        for (var i = 0; i < 7; i++)
            Engine.World.Keys[i] = true;
    }
}