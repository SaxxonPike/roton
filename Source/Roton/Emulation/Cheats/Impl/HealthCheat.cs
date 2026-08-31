using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "HEALTH" cheat, which increases the player's health.
/// </summary>
[Context(Context.Original, "HEALTH")]
[Context(Context.Super, "HEALTH")]
internal sealed class HealthCheat(
    IWorld world) 
    : ICheat
{
    public void Execute(bool clear) => 
        world.Health += 50;
}