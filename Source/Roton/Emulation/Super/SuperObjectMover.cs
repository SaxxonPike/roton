using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperObjectMover : IObjectMover
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public SuperObjectMover(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public void ExecuteDirection(IOopContext context, IXyPair vector)
    {
        var count = Engine.Parser.ReadNumber(context.Index, context);
        if (count < 0)
            count = 1;

        if (context.Command == 0x3F) // ?
            count = -count;

        context.Actor.P2 = count;
        context.Actor.Vector.CopyFrom(vector);
        context.Repeat = false;
    }
        
}