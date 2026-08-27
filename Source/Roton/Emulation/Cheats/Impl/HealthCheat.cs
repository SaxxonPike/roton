using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "HEALTH")]
[Context(Context.Super, "HEALTH")]
public sealed class HealthCheat(
    IWorld world) 
    : ICheat
{
    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        world.Health += 50;
    }
}