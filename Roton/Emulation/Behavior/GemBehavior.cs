using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal sealed class GemBehavior : ElementBehavior
    {
        private readonly int _healthPerGem;

        public GemBehavior(int healthPerGem)
        {
            _healthPerGem = healthPerGem;
        }

        public override string KnownName => "Gem";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.World.Health += _healthPerGem;
            engine.World.Gems += 1;
            engine.World.Score += 10;
            engine.RemoveItem(location);
            engine.UpdateStatus();
            engine.PlaySound(2, engine.SoundSet.Gem);

            if (!engine.Alerts.GemPickup)
                return;

            engine.SetMessage(0xC8, engine.Alerts.GemMessage);
            engine.Alerts.GemPickup = false;
        }
    }
}