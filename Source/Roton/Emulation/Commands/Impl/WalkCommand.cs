using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "WALK")]
[Context(Context.Super, "WALK")]
public sealed class WalkCommand(
    IEngineAccessor engine,
    IParser parser)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (parser.TryEvalDirection(ref context, ref instruction, out var vec)) 
            context.Actor.Vector = vec;
    }
}