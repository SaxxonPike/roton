using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "SCORE")]
[Context(Context.Super, "SCORE")]
public sealed class ScoreItem(IEngineAccessor engine) : IItem
{
    public ref Word Value => ref engine.Instance.World.Score;
}