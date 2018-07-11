using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Zzt, "TORCHES")]
    public sealed class TorchesCheat : ICheat
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