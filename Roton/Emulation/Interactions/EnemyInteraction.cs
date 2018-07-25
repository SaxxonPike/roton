using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Original, 0x0F)]
    [ContextEngine(ContextEngine.Original, 0x12)]
    [ContextEngine(ContextEngine.Original, 0x22)]
    [ContextEngine(ContextEngine.Original, 0x23)]
    [ContextEngine(ContextEngine.Original, 0x29)]
    [ContextEngine(ContextEngine.Original, 0x2A)]
    [ContextEngine(ContextEngine.Original, 0x2C)]
    [ContextEngine(ContextEngine.Original, 0x2D)]
    [ContextEngine(ContextEngine.Super, 0x22)]
    [ContextEngine(ContextEngine.Super, 0x23)]
    [ContextEngine(ContextEngine.Super, 0x29)]
    [ContextEngine(ContextEngine.Super, 0x2A)]
    [ContextEngine(ContextEngine.Super, 0x2C)]
    [ContextEngine(ContextEngine.Super, 0x2D)]
    [ContextEngine(ContextEngine.Super, 0x3B)]
    [ContextEngine(ContextEngine.Super, 0x3C)]
    [ContextEngine(ContextEngine.Super, 0x3D)]
    [ContextEngine(ContextEngine.Super, 0x3E)]
    [ContextEngine(ContextEngine.Super, 0x45)]
    [ContextEngine(ContextEngine.Super, 0x48)]
    public sealed class EnemyInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public EnemyInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Attack(index, location);
        }
    }
}