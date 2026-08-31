using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the counter-clockwise conveyor element.
/// </summary>
[Context(Context.Original, 0x11)]
[Context(Context.Super, 0x11)]
internal sealed class CounterclockwiseConveyorAction(
    IActorList actorList,
    IConveyor conveyor,
    IBoardUpdater boardUpdater)
    : IAction
{
    public void Act(int index)
    {
        var actor = actorList[index];
        boardUpdater.UpdateBoard(actor.Location);
        conveyor.Convey(actor.Location, -1);
    }
}