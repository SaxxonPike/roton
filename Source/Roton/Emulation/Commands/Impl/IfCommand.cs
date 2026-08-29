using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "IF")]
[Context(Context.Super, "IF")]
internal sealed class IfCommand(
    IParser parser)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (parser.TryEvalCondition(ref context, ref instruction, out var result))
            context.Resume = result;
    }
}