using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.SuperZzt, "NOZ")]
    public sealed class NoZCheat : ICheat
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