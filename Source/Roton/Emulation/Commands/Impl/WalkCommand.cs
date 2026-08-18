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
        var vector = Engine.Parser.GetDirection(ref context, ref instruction);
        if (vector is {} vec)
        {
            context.Actor.Vector = vec;
        }
    }
}