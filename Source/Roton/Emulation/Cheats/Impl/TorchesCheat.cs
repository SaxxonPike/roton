using System;
using Roton.Emulation.Core;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "TORCHES")]
public sealed class TorchesCheat(IEngineAccessor engine) : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        Engine.World.Torches += 3;
    }
}