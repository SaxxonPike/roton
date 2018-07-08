using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class ThrowstarCommand : ICommand
    {
        private readonly IEngine _engine;

        public ThrowstarCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "THROWSTAR";
        
        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            if (vector != null)
            {
                var projectile = _engine.Elements.Star();
                _engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
            }
            context.Moved = true;
        }
    }
}