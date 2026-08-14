using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x11)]
[Context(Context.Super, 0x11)]
public sealed class CounterclockwiseConveyorAction(Lazy<IEngine> engine) : IAction
{
    private IEngine Engine => engine.Value;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        Engine.UpdateBoard(actor.Location);
        Engine.Convey(actor.Location, -1);
    }
}