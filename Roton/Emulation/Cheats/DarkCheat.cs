using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Cheats
{
    public class DarkCheat : ICheat
    {
        private readonly IEngine _engine;

        public DarkCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "DARK";
        
        public void Execute()
        {
            _engine.Board.IsDark = true;
            _engine.Hud.RedrawBoard();
        }
    }
}