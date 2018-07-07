using Roton.Core;

namespace Roton.Emulation.Commands
{
    public class EndgameCommand : ICommand
    {
        private readonly IWorld _world;

        public EndgameCommand(IWorld world)
        {
            _world = world;
        }
        
        public string Name => "ENDGAME";
        
        public void Execute(IOopContext context)
        {
            _world.Health = 0;
        }
    }
}