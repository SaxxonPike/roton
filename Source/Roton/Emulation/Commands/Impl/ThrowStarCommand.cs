using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "THROWSTAR")]
[Context(Context.Super, "THROWSTAR")]
internal sealed class ThrowStarCommand(
    IParser parser,
    IElementList elementList,
    ISpawner spawner)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (parser.TryEvalDirection(ref context, ref instruction, out var vec))
        {
            var projectile = elementList.Star();
            spawner.SpawnProjectile(projectile.Id, context.Actor.Location, vec, true);
        }

        context.Moved = true;
    }
}