using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Original, "ENDGAME")]
    [ContextEngine(ContextEngine.Super, "ENDGAME")]
    public sealed class EndgameCommand : ICommand
    {
        private readonly IEngine _engine;

        public EndgameCommand(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(IOopContext context)
        {
            _engine.World.Health = 0;
        }
    }
}