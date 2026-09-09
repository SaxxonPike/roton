using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the transporter element.
/// </summary>
[Context(Context.Original, id: 0x1E)]
[Context(Context.Super, id: 0x1E)]
internal sealed class TransporterAction(
    IActorList actors,
    IBoardUpdater boardUpdater)
    : IAction
{
    /// <remarks>
    /// RoZ: ElementTransporterTick
    /// </remarks>
    public void Act(int index) => 
        boardUpdater.UpdateBoard(actors[index].Location);
}