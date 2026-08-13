using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "SCORE")]
[Context(Context.Super, "SCORE")]
public sealed class ScoreItem(Lazy<IEngine> engine) : IItem
{
    private IEngine Engine => engine.Value;

    public int Value
    {
        get => Engine.World.Score;
        set => Engine.World.Score = value;
    }
}