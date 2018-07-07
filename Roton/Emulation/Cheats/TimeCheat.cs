using Roton.Core;

namespace Roton.Emulation.Cheats
{
    public class TimeCheat : ICheat
    {
        private readonly IWorld _world;

        public TimeCheat(IWorld world)
        {
            _world = world;
        }
        
        public string Name => "TIME";
        
        public void Execute()
        {
            _world.TimePassed -= 30;
        }
    }
}