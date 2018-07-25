using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "WALK")]
    [ContextEngine(ContextEngine.Super, "WALK")]
    public sealed class WalkCommand : ICommand
    {
        private readonly IEngine _engine;

        public WalkCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            var vector = _engine.Parser.GetDirection(context);
            if (vector != null)
            {
                context.Actor.Vector.CopyFrom(vector);
            }
        }
    }
}