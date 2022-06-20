using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Super, "Z")]
public sealed class ZItem : IItem
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public ZItem(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public int Value
    {
        get => Engine.World.Stones;
        set => Engine.World.Stones = value;
    }
}