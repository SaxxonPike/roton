using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BECOME")]
[Context(Context.Super, "BECOME")]
internal sealed class BecomeCommand(
    IEngineAccessor engine,
    IParser parser)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (!parser.TryEvalKind(ref context, ref instruction, out var val))
        {
            Engine.RaiseError(ref context, "Bad #BECOME");
            return;
        }

        context.Died = true;
        context.DeathTile = val;
    }
}