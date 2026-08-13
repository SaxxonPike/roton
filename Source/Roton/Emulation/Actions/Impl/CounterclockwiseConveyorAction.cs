using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x11)]
[Context(Context.Super, 0x11)]
public sealed class CounterclockwiseConveyorAction : IAction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public CounterclockwiseConveyorAction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        Engine.UpdateBoard(actor.Location);
        Engine.Convey(actor.Location, -1);
    }
}