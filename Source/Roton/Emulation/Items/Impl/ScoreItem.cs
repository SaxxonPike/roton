using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "SCORE")]
[Context(Context.Super, "SCORE")]
public sealed class ScoreItem(IEngineAccessor engine) : IItem
{
    private IEngine Engine => engine.Instance;

    public int Value
    {
        get => Engine.World.Score;
        set => Engine.World.Score = value;
    }
}