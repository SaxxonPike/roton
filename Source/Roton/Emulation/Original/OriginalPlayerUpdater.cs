using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalPlayerUpdater(
    IActorList actorList,
    IElementList elementList,
    IFacts facts,
    ITiles tiles,
    IBoardUpdater boardUpdater)
    : IPlayerUpdater
{
    public void ForcePlayerColor(int index)
    {
        var actor = actorList[index];
        var playerElement = elementList.Player();
        if (tiles[actor.Location].Color == playerElement.Color &&
            playerElement.Character == facts.PlayerCharacter)
            return;

        playerElement.Character = facts.PlayerCharacter;
        tiles[actor.Location].Color = playerElement.Color;
        boardUpdater.UpdateBoard(actor.Location);
    }

    public void CleanUpPauseMovement()
    {
        // No-op in the original engine.
    }
}