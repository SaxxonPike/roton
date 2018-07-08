using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Cheats
{
    public class TimeCheat : ICheat
    {
        private readonly IEngine _engine;

        public TimeCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "TIME";
        
        public void Execute()
        {
            _engine.World.TimePassed -= 30;
        }
    }
}