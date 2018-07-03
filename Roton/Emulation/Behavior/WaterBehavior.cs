using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public class WaterBehavior : ElementBehavior
    {
        public override string KnownName => "Water";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.PlaySound(3, engine.SoundSet.Water);
            engine.SetMessage(0x64, engine.Alerts.WaterMessage);
        }
    }
}