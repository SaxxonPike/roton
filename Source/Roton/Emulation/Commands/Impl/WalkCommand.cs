using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "WALK")]
[Context(Context.Super, "WALK")]
public sealed class WalkCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var vector = Engine.Parser.GetDirection(context);
        if (vector is {} vec)
        {
            context.Actor.Vector = vec;
        }
    }
}