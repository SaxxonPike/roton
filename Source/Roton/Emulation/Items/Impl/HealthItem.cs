using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "HEALTH")]
[Context(Context.Super, "HEALTH")]
public sealed class HealthItem : IItem
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public HealthItem(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public int Value
    {
        get => Engine.World.Health;
        set => Engine.World.Health = value;
    }
}