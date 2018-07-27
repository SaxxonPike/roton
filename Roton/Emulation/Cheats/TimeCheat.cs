using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "TIME")]
    [ContextEngine(ContextEngine.Super, "TIME")]
    public sealed class TimeCheat : ICheat
    {
        private readonly IEngine _engine;

        public TimeCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute(string name, bool clear)
        {
            _engine.World.TimePassed -= 30;
        }
    }
}