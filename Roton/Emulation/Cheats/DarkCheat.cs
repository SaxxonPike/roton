using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "DARK")]
    public sealed class DarkCheat : ICheat
    {
        private readonly IEngine _engine;

        public DarkCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(string name, bool clear)
        {
            _engine.Board.IsDark = !clear;
            _engine.Hud.RedrawBoard();
        }
    }
}