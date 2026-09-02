using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "PUT")]
[Context(Context.Super, "PUT")]
internal sealed class PutCommand(
    IEngineAccessor engine,
    IParser parser,
    IErrorRaiser errorRaiser,
    IPlotter plotter)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var success = false;

        if (parser.TryEvalDirection(ref context, ref instruction, out var vec))
        {
            if (parser.TryEvalKind(ref context, ref instruction, out var k))
            {
                success = true;

                var target = context.Actor.Location + vec;
                plotter.Put(target, vec, k);
            }
        }

        if (!success)
            errorRaiser.RaiseError(ref context, "Bad #PUT");
    }
}