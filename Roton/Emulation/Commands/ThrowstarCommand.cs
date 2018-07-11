using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt, "THROWSTAR")]
    [ContextEngine(ContextEngine.SuperZzt, "THROWSTAR")]
    public sealed class ThrowstarCommand : ICommand
    {
        private readonly IEngine _engine;

        public ThrowstarCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            if (vector != null)
            {
                var projectile = _engine.ElementList.Star();
                _engine.SpawnProjectile(projectile.Id, context.Actor.Location, vector, true);
            }
            context.Moved = true;
        }
    }
}