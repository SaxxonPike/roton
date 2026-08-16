using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x10)]
[Context(Context.Super, 0x10)]
public sealed class ClockwiseConveyorAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        Engine.UpdateBoard(actor.Location);
        Engine.Convey(actor.Location, 1);
    }
}