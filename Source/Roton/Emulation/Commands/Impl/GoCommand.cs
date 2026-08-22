using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "GO")]
[Context(Context.Super, "GO")]
public sealed class GoCommand(
    IParser parser,
    IEngineAccessor engine,
    ITiles tiles)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (!parser.TryEvalDirection(ref context, ref instruction, out var vec))
            return;

        var target = context.Actor.Location + vec;

        if (!tiles.ElementAt(target).IsFloor)
            Engine.Push(target, vec);

        if (tiles.ElementAt(target).IsFloor)
        {
            Engine.MoveActor(context.Index, target);
            context.Moved = true;
        }
        else
        {
            context.Repeat = true;
        }
    }
}