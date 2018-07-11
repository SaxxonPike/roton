using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Zzt, "GEMS")]
    [ContextEngine(ContextEngine.SuperZzt, "GEMS")]
    public sealed class GemsCheat : ICheat
    {
        private readonly IEngine _engine;

        public GemsCheat(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "GEMS";
        
        public void Execute()
        {
            _engine.World.Gems += 5;
        }
    }
}