using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CHAR")]
[Context(Context.Super, "CHAR")]
internal sealed class CharCommand(
    IParser parser,
    IBoardUpdater boardUpdater)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        var value = parser.ReadNumber(context.Index, ref instruction);

        if (value < 0)
            return;

        context.Actor.P1 = value;
        boardUpdater.UpdateBoard(context.Actor.Location);
    }
}