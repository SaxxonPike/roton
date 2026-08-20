using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "THROWSTAR")]
[Context(Context.Super, "THROWSTAR")]
public sealed class ThrowstarCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (Engine.Parser.TryEvalDirection(ref context, ref instruction, out var vec))
        {
            var projectile = Engine.ElementList.Star();
            Engine.SpawnProjectile(projectile.Id, context.Actor.Location, vec, true);
        }

        context.Moved = true;
    }
}