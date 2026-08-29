using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Super, "Z")]
internal sealed class ZCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear)
    {
        world.Stones++;
    }
}