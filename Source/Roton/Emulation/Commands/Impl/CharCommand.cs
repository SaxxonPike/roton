using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHAR")]
[Context(Context.Super, "CHAR")]
public sealed class CharCommand(
    IEngineAccessor engine,
    IParser parser,
    IBoardUpdater boardUpdater)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var value = parser.ReadNumber(context.Index, ref instruction);
        if (value >= 0)
        {
            context.Actor.P1 = unchecked((byte)value);
            boardUpdater.UpdateBoard(context.Actor.Location);
        }
    }
}