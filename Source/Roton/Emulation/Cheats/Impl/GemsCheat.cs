using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "GEMS")]
[Context(Context.Super, "GEMS")]
internal sealed class GemsCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear)
    {
        world.Gems += 5;
    }
}