using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the clockwise conveyor element.
/// </summary>
[Context(Context.Original, id: 0x10)]
[Context(Context.Super, id: 0x10)]
internal sealed class ClockwiseConveyorAction(
    IActorList actors,
    IConveyor conveyor,
    IBoardUpdater boardUpdater) 
    : IAction
{
    /// <remarks>
    /// RoZ: ElementConveyorCWTick
    /// </remarks>
    public void Act(int index)
    {
        var actor = actors[index];
        boardUpdater.UpdateBoard(actor.Location);
        conveyor.Convey(actor.Location, 1);
    }
}