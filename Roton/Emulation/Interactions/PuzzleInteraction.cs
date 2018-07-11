using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt, 0x18)]
    [ContextEngine(ContextEngine.Zzt, 0x19)]
    [ContextEngine(ContextEngine.Zzt, 0x1A)]
    [ContextEngine(ContextEngine.SuperZzt, 0x18)]
    [ContextEngine(ContextEngine.SuperZzt, 0x19)]
    [ContextEngine(ContextEngine.SuperZzt, 0x1A)]
    public sealed class PuzzleInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public PuzzleInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Push(location, vector);
            _engine.PlaySound(2, _engine.Sounds.Push);
        }
    }
}