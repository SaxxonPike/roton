using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Commands
{
    public class EndgameCommand : ICommand
    {
        private readonly IEngine _engine;

        public EndgameCommand(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "ENDGAME";
        
        public void Execute(IOopContext context)
        {
            _engine.World.Health = 0;
        }
    }
}