using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "HEALTH")]
[Context(Context.Super, "HEALTH")]
public sealed class HealthCheat : ICheat
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;
        
    public HealthCheat(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Execute(string name, bool clear)
    {
        Engine.World.Health += 50;
    }
}