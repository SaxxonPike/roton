using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl
{
    [Context(Context.Original, "SHOOT")]
    [Context(Context.Super, "SHOOT")]
    public sealed class ShootCommand : ICommand
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public ShootCommand(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var vector = Engine.Parser.GetDirection(context);
            if (vector != null)
            {
                var projectile = Engine.ElementList.Bullet();
                var success = Engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
                if (success)
                {
                    Engine.PlaySound(2, Engine.Sounds.EnemyShoot);
                }
                context.Moved = true;
            }
        }
    }
}