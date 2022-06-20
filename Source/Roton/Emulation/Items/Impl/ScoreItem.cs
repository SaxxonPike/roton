using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "SCORE")]
[Context(Context.Super, "SCORE")]
public sealed class ScoreItem : IItem
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public ScoreItem(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public int Value
    {
        get => Engine.World.Score;
        set => Engine.World.Score = value;
    }
}