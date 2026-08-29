using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "TIME")]
[Context(Context.Super, "TIME")]
internal sealed class TimeCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear)
    {
        world.TimePassed -= 30;
    }
}