using System;
using Roton.Emulation.Core;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "HEALTH")]
[Context(Context.Super, "HEALTH")]
public sealed class HealthCheat(IEngineAccessor engine) : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        Engine.World.Health += 50;
    }
}