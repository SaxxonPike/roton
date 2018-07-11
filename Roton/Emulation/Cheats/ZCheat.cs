using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.SuperZzt, "Z")]
    public sealed class ZCheat : ICheat
    {
        private readonly IEngine _engine;

        public ZCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "Z";
        
        public void Execute()
        {
            _engine.World.Stones++;
        }
    }
}