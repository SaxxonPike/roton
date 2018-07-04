using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public class WaterBehavior : ElementBehavior
    {
        public override string KnownName => "Water";

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PlaySound(3, _sounds.Water);
            engine.SetMessage(0x64, _alerts.WaterMessage);
        }
    }
}