using Roton.Emulation.Core;

namespace Roton.Emulation.Cheats
{
    public class TorchesCheat : ICheat
    {
        private readonly IEngine _engine;

        public TorchesCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "TORCHES";
        
        public void Execute()
        {
            _engine.World.Torches += 3;
        }
    }
}