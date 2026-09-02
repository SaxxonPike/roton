using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "ZAP" cheat, which destroys four adjacent tiles around the player.
/// </summary>
[Context(Context.Original, "ZAP")]
[Context(Context.Super, "ZAP")]
internal sealed class ZapCheat(
    IEngineAccessor engine,
    IActorList actors,
    IState state,
    IAttacker attacker)
    : ICheat
{
    private IEngine Engine => engine.Instance;

    public void Execute(bool clear)
    {
        for (var i = 0; i < 4; i++)
            attacker.Destroy(actors.Player.Location + state.GetCardinalVector(i));
    }
}