using Roton.Emulation.Core;

namespace Roton.Emulation.Cheats
{
    public class KeysCheat : ICheat
    {
        private readonly IEngine _engine;

        public KeysCheat(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "KEYS";
        
        public void Execute()
        {
            for (var i = 1; i < 8; i++)
                _engine.World.Keys[i] = true;
        }
    }
}