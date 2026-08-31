using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the clockwise conveyor element.
/// </summary>
[Context(Context.Original, 0x10)]
[Context(Context.Super, 0x10)]
internal sealed class ClockwiseConveyorAction(
    IActorList actors,
    IConveyor conveyor,
    IBoardUpdater boardUpdater) 
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];
        boardUpdater.UpdateBoard(actor.Location);
        conveyor.Convey(actor.Location, 1);
    }
}