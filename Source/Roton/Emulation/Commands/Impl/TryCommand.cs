using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "TRY")]
[Context(Context.Super, "TRY")]
public sealed class TryCommand : ICommand
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public TryCommand(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Execute(IOopContext context)
    {
        var vector = Engine.Parser.GetDirection(context);
        if (vector == null)
            return;

        var target = vector.Sum(context.Actor.Location);
        if (!Engine.Tiles.ElementAt(target).IsFloor)
        {
            Engine.Push(target, vector);
        }
        if (Engine.ElementAt(target).IsFloor)
        {
            Engine.MoveActor(context.Index, target);
            context.Moved = true;
            context.Resume = false;
        }
        else
        {
            context.Resume = true;
        }
    }
}