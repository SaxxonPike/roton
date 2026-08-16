using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHAR")]
[Context(Context.Super, "CHAR")]
public sealed class CharCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var value = Engine.Parser.ReadNumber(context.Index, context);
        if (value >= 0)
        {
            context.Actor.P1 = value;
            Engine.UpdateBoard(context.Actor.Location);
        }
    }
}