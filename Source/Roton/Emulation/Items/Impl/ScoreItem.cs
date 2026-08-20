using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "SCORE")]
[Context(Context.Super, "SCORE")]
public sealed class ScoreItem(IEngineAccessor engine) : IItem
{
    public ref Word Value => ref engine.Instance.World.Score;
}