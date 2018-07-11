using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt, 0x13)]
    [ContextEngine(ContextEngine.SuperZzt, 0x13)]
    public sealed class WaterInteraction : IInteraction
    {
        private readonly IEngine _engine;
        
        public WaterInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PlaySound(3, _engine.Sounds.Water);
            _engine.SetMessage(0x64, _engine.Alerts.WaterMessage);
        }
    }
}