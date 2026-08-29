using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "TORCHES")]
internal sealed class TorchesCheat(
    IWorld world)
    : ICheat
{
    public void Execute(bool clear)
    {
        world.Torches += 3;
    }
}