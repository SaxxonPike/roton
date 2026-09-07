using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Directions;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "THROWSTAR")]
[Context(Context.Super, "THROWSTAR")]
internal sealed class ThrowStarCommand(
    IElementList elements,
    ISpawner spawner,
    IDirectionEvaluator directionEvaluator)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (directionEvaluator.TryEval(ref context, ref instruction, out var vec))
            spawner.SpawnProjectile(elements.StarId, context.Actor.Location, vec, true);

        context.Moved = true;
    }
}