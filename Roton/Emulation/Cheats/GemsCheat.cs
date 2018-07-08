using Roton.Core;

namespace Roton.Emulation.Cheats
{
    public class GemsCheat : ICheat
    {
        private readonly IEngine _engine;

        public GemsCheat(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "GEMS";
        
        public void Execute()
        {
            _engine.World.Gems += 5;
        }
    }
}