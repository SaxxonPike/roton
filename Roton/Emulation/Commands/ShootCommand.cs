using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Commands
{
    public class ShootCommand : ICommand
    {
        private readonly IEngine _engine;

        public ShootCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "SHOOT";
        
        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            if (vector != null)
            {
                var projectile = _engine.Elements.Bullet();
                var success = _engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
                if (success)
                {
                    _engine.PlaySound(2, _engine.Sounds.EnemyShoot);
                }
                context.Moved = true;
            }
        }
    }
}