using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "SCORE")]
[Context(Context.Super, "SCORE")]
internal sealed class ScoreItem(
    IWorld world)
    : IItem
{
    public ref Word Value => ref world.Score;
}