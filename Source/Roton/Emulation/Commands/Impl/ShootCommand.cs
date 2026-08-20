using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "SHOOT")]
[Context(Context.Super, "SHOOT")]
public sealed class ShootCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        if (!Engine.Parser.TryEvalDirection(ref context, ref instruction, out var vec))
            return;

        var projectile = Engine.Elements.Bullet();
        var success = Engine.SpawnProjectile(projectile.Id, context.Actor.Location, vec, true);

        if (success)
            Engine.PlaySound(2, Engine.Sounds.EnemyShoot);

        context.Moved = true;
    }
}