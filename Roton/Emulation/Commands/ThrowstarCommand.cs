using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class ThrowstarCommand : ICommand
    {
        private readonly IParser _parser;
        private readonly IElements _elements;
        private readonly ISpawner _spawner;

        public ThrowstarCommand(IParser parser, IElements elements, ISpawner spawner)
        {
            _parser = parser;
            _elements = elements;
            _spawner = spawner;
        }
        
        public string Name => "THROWSTAR";
        
        public void Execute(IOopContext context)
        {
            var vector = _parser.GetDirection(context);
            if (vector != null)
            {
                var projectile = _elements.Star();
                _spawner.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
            }
            context.Moved = true;
        }
    }
}