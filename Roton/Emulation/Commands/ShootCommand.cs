using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class ShootCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IElements _elements;
        private readonly ISpawner _spawner;
        private readonly ISounder _sounder;
        private readonly ISounds _sounds;

        public ShootCommand(IParser parser, IElements elements, ISpawner spawner, ISounder sounder, ISounds sounds)
        {
            _parser = parser;
            _elements = elements;
            _spawner = spawner;
            _sounder = sounder;
            _sounds = sounds;
        }
        
        public string Name => "SHOOT";
        
        public void Execute(IOopContext context)
        {
            var vector = _parser.GetDirection(context);
            if (vector != null)
            {
                var projectile = _elements.Bullet();
                var success = _spawner.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
                if (success)
                {
                    _sounder.Play(2, _sounds.EnemyShoot);
                }
                context.Moved = true;
            }
        }
    }
}