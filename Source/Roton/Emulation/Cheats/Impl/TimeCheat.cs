using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "TIME")]
[Context(Context.Super, "TIME")]
public sealed class TimeCheat(
    IEngineAccessor engine,
    IWorld world)
    : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        world.TimePassed -= 30;
    }
}