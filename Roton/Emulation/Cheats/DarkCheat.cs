using Roton.Core;

namespace Roton.Emulation.Cheats
{
    public class DarkCheat : ICheat
    {
        private readonly IBoard _board;
        private readonly IHud _hud;

        public DarkCheat(IBoard board, IHud hud)
        {
            _board = board;
            _hud = hud;
        }
        
        public string Name => "DARK";
        
        public void Execute()
        {
            _board.IsDark = true;
            _hud.RedrawBoard();
        }
    }
}