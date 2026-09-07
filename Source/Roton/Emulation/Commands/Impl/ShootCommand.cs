using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Directions;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "SHOOT")]
[Context(Context.Super, "SHOOT")]
internal sealed class ShootCommand(
    IElementList elements,
    ISounds sounds,
    ISoundPlayer soundPlayer,
    ISpawner spawner,
    IDirectionEvaluator directionEvaluator)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (!directionEvaluator.TryEval(ref context, ref instruction, out var vec))
            return;

        var success = spawner.SpawnProjectile(elements.BulletId, context.Actor.Location, vec, true);

        if (success)
            soundPlayer.PlaySound(2, sounds.EnemyShoot);

        context.Moved = true;
    }
}