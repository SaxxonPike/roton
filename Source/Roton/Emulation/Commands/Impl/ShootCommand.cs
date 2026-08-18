using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "SHOOT")]
[Context(Context.Super, "SHOOT")]
public sealed class ShootCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var vector = Engine.Parser.GetDirection(context);
        if (vector is {} vec)
        {
            var projectile = Engine.ElementList.Bullet();
            var success = Engine.SpawnProjectile(projectile.Id, context.Actor.Location, vec, true);
            if (success)
            {
                Engine.PlaySound(2, Engine.Sounds.EnemyShoot);
            }
            context.Moved = true;
        }
    }
}