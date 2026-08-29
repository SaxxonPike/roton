using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x10)]
[Context(Context.Super, 0x10)]
internal sealed class ClockwiseConveyorAction(
    IActorList actorList,
    IConveyor conveyor,
    IBoardUpdater boardUpdater) 
    : IAction
{
    public void Act(int index)
    {
        var actor = actorList[index];
        boardUpdater.UpdateBoard(actor.Location);
        conveyor.Convey(actor.Location, 1);
    }
}