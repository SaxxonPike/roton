using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "WALK")]
[Context(Context.Super, "WALK")]
public sealed class WalkCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (Engine.Parser.TryEvalDirection(ref context, ref instruction, out var vec)) 
            context.Actor.Vector = vec;
    }
}