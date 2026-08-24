using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterAction(
    IActorList actorList,
    IBoardUpdater boardUpdater)
    : IAction
{
    public void Act(int index)
    {
        boardUpdater.UpdateBoard(actorList[index].Location);
    }
}