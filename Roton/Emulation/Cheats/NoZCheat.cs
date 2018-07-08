using Roton.Emulation.Core;

namespace Roton.Emulation.Cheats
{
    public class NoZCheat : ICheat
    {
        private readonly IEngine _engine;

        public NoZCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "NOZ";
        
        public void Execute()
        {
            _engine.World.Stones--;
        }
    }
}