using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt, 0x0F)]
    [ContextEngine(ContextEngine.Zzt, 0x12)]
    [ContextEngine(ContextEngine.Zzt, 0x22)]
    [ContextEngine(ContextEngine.Zzt, 0x23)]
    [ContextEngine(ContextEngine.Zzt, 0x29)]
    [ContextEngine(ContextEngine.Zzt, 0x2A)]
    [ContextEngine(ContextEngine.Zzt, 0x2C)]
    [ContextEngine(ContextEngine.Zzt, 0x2D)]
    [ContextEngine(ContextEngine.SuperZzt, 0x22)]
    [ContextEngine(ContextEngine.SuperZzt, 0x23)]
    [ContextEngine(ContextEngine.SuperZzt, 0x29)]
    [ContextEngine(ContextEngine.SuperZzt, 0x2A)]
    [ContextEngine(ContextEngine.SuperZzt, 0x2C)]
    [ContextEngine(ContextEngine.SuperZzt, 0x2D)]
    [ContextEngine(ContextEngine.SuperZzt, 0x3B)]
    [ContextEngine(ContextEngine.SuperZzt, 0x3C)]
    [ContextEngine(ContextEngine.SuperZzt, 0x3D)]
    [ContextEngine(ContextEngine.SuperZzt, 0x3E)]
    [ContextEngine(ContextEngine.SuperZzt, 0x45)]
    [ContextEngine(ContextEngine.SuperZzt, 0x48)]
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