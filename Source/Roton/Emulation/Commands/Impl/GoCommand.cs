using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "GO")]
[Context(Context.Super, "GO")]
public sealed class GoCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var vector = Engine.Parser.GetDirection(ref context, ref instruction);
        if (vector is {} vec)
        {
            var target = context.Actor.Location + vec;
            if (!Engine.Tiles.ElementAt(target).IsFloor)
            {
                Engine.Push(target, vec);
            }
            if (Engine.Tiles.ElementAt(target).IsFloor)
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
}