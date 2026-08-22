using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterAction(
    IEngineAccessor engine,
    IActorList actorList)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        Engine.UpdateBoard(actorList[index].Location);
    }
}