using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "WALK")]
[Context(Context.Super, "WALK")]
internal sealed class WalkCommand(
    IParser parser)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (parser.TryEvalDirection(ref context, ref instruction, out var vec)) 
            context.Actor.Vector = vec;
    }
}