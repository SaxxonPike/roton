using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Behaviors
{
    public class WaterBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Water;

        public WaterBehavior(IEngine engine)
        {
            _engine = engine;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PlaySound(3, _engine.Sounds.Water);
            _engine.SetMessage(0x64, _engine.Alerts.WaterMessage);
        }
    }
}