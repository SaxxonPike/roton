using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "TRY")]
[Context(Context.Super, "TRY")]
public sealed class TryCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var vector = Engine.Parser.GetDirection(context);
        if (vector is not {} vec)
            return;

        var target = context.Actor.Location + vec;
        if (!Engine.Tiles.ElementAt(target).IsFloor)
        {
            Engine.Push(target, vec);
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