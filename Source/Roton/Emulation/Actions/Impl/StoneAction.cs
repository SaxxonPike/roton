using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Super, 0x40)]
public sealed class StoneAction(
    IActorList actorList,
    IRandomizer randomizer,
    ITiles tiles,
    IBoardUpdater boardUpdater) 
    : IAction
{
    public void Act(int index)
    {
        var actor = actorList[index];
        tiles[actor.Location].Color =
            (tiles[actor.Location].Color & 0x70) + randomizer.GetNext(7) + 9;
        boardUpdater.UpdateBoard(actorList[index].Location);
    }
}