using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BECOME")]
[Context(Context.Super, "BECOME")]
public sealed class BecomeCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var kind = Engine.Parser.GetKind(context);
        if (kind == null)
        {
            Engine.RaiseError("Bad #BECOME");
            return;
        }

        context.Died = true;
        context.DeathTile.CopyFrom(kind);
    }
}