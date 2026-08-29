using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Super, "NOZ")]
internal sealed class NoZCheat(
    IWorld world)
    : ICheat
{
    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        world.Stones = -1;
    }
}