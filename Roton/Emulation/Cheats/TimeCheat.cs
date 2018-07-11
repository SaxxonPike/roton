using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Zzt, "TIME")]
    [ContextEngine(ContextEngine.SuperZzt, "TIME")]
    public sealed class TimeCheat : ICheat
    {
        private readonly IEngine _engine;

        public TimeCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            _engine.World.TimePassed -= 30;
        }
    }
}