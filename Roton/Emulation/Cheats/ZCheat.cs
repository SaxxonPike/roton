using Roton.Emulation.Core;

namespace Roton.Emulation.Cheats
{
    public class ZCheat : ICheat
    {
        private readonly IEngine _engine;

        public ZCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "Z";
        
        public void Execute()
        {
            _engine.World.Stones++;
        }
    }
}