using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "CYCLE")]
[Context(Context.Super, "CYCLE")]
public sealed class CycleCommand(
    IParser parser)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        var value = parser.ReadNumber(context.Index, ref instruction);
        if (value > 0)
        {
            context.Actor.Cycle = value;
        }
    }
}