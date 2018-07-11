using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Zzt, "DARK")]
    public sealed class DarkCheat : ICheat
    {
        private readonly IEngine _engine;

        public DarkCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _engine.Board.IsDark = true;
            _engine.Hud.RedrawBoard();
        }
    }
}