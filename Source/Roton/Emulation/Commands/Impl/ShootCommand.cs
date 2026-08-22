using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "SHOOT")]
[Context(Context.Super, "SHOOT")]
public sealed class ShootCommand(
    IEngineAccessor engine,
    IParser parser,
    IElementList elementList,
    ISounds sounds)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (!parser.TryEvalDirection(ref context, ref instruction, out var vec))
            return;

        var projectile = elementList.Bullet();
        var success = Engine.SpawnProjectile(projectile.Id, context.Actor.Location, vec, true);

        if (success)
            Engine.PlaySound(2, sounds.EnemyShoot);

        context.Moved = true;
    }
}