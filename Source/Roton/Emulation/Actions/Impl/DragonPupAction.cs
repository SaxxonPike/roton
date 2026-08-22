using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Super, 0x3C)]
public sealed class DragonPupAction(
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