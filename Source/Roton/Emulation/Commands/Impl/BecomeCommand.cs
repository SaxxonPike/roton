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
        var kind = Engine.Parser.GetKind(ref context, ref instruction);
        if (kind is not {} val)
        {
            Engine.RaiseError("Bad #BECOME");
            return;
        }

        context.Died = true;
        context.DeathTile = val;
    }
}