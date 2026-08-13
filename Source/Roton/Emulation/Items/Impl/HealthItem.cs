using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "HEALTH")]
[Context(Context.Super, "HEALTH")]
public sealed class HealthItem(Lazy<IEngine> engine) : IItem
{
    private IEngine Engine => engine.Value;

    public int Value
    {
        get => Engine.World.Health;
        set => Engine.World.Health = value;
    }
}