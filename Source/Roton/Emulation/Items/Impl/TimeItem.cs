using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Items.Impl;

[Context(Context.Original, "TIME")]
[Context(Context.Super, "TIME")]
internal sealed class TimeItem(
    IWorld world)
    : IItem
{
    public ref Word Value => ref world.TimePassed;
}