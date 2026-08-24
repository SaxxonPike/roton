using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "TIME")]
[Context(Context.Super, "TIME")]
public sealed class TimeCheat(
    IWorld world)
    : ICheat
{
    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        world.TimePassed -= 30;
    }
}