using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Super, 0x3C)]
public sealed class DragonPupAction(
    IActorList actorList,
    IBoardUpdater boardUpdater) 
    : IAction
{
    public void Act(int index)
    {
        boardUpdater.UpdateBoard(actorList[index].Location);
    }
}