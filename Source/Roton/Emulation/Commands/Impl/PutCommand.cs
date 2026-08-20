using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "PUT")]
[Context(Context.Super, "PUT")]
public sealed class PutCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var success = false;

        if (Engine.Parser.TryEvalDirection(ref context, ref instruction, out var vec))
        {
            if (Engine.Parser.TryEvalKind(ref context, ref instruction, out var k))
            {
                success = true;

                var target = context.Actor.Location + vec;
                Engine.PutTile(target, vec, k);
            }
        }

        if (!success)
            Engine.RaiseError(ref context, "Bad #PUT");
    }
}