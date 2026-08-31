using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperPlayerUpdater(
    IActorList actors,
    ITiles tiles)
    : IPlayerUpdater
{
    public void ForcePlayerColor(int index)
    {
        // No-op in the super engine.
    }

    public void CleanUpPauseMovement() => 
        actors.Player.UnderTile = tiles[actors.Player.Location];
    
    public void CleanUpPassageMovement() => 
        tiles[actors.Player.Location] = actors.Player.UnderTile;
}