using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "TORCHES")]
public sealed class TorchesItem : IItem
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public TorchesItem(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public int Value
    {
        get => Engine.World.Torches;
        set => Engine.World.Torches = value;
    }
}