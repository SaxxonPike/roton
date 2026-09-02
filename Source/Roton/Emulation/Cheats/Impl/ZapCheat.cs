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
    IActorList actors,
    IState state,
    IDestroyer destroyer)
    : ICheat
{
    public void Execute(bool clear)
    {
        for (var i = 0; i < 4; i++)
            destroyer.Destroy(actors.Player.Location + state.GetCardinalVector(i));
    }
}