using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BECOME")]
[Context(Context.Super, "BECOME")]
public sealed class BecomeCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (!Engine.Parser.TryEvalKind(ref context, ref instruction, out var val))
        {
            Engine.RaiseError("Bad #BECOME");
            return;
        }

        context.Died = true;
        context.DeathTile = val;
    }
}