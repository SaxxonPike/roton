using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHAR")]
[Context(Context.Super, "CHAR")]
public sealed class CharCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var value = Engine.Parser.ReadNumber(context.Index, ref instruction);
        if (value >= 0)
        {
            context.Actor.P1 = unchecked((byte)value);
            Engine.UpdateBoard(context.Actor.Location);
        }
    }
}