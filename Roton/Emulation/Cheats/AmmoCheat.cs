using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Zzt, "AMMO")]
    [ContextEngine(ContextEngine.SuperZzt, "AMMO")]
    public sealed class AmmoCheat : ICheat
    {
        private readonly IEngine _engine;

        public AmmoCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "AMMO";
        
        public void Execute()
        {
            _engine.World.Ammo += 5;
        }
    }
}