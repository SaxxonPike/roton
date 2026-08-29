using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "TORCHES")]
internal sealed class TorchesCheat(
    IWorld world)
    : ICheat
{
    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        world.Torches += 3;
    }
}