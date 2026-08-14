using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "HEALTH")]
[Context(Context.Super, "HEALTH")]
public sealed class HealthCheat(Lazy<IEngine> engine) : ICheat
{
    private IEngine Engine => engine.Value;

    public void Execute(string name, bool clear)
    {
        Engine.World.Health += 50;
    }
}