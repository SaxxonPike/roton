using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Behaviors
{
    public sealed class GemBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public GemBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override string KnownName => KnownNames.Gem;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.World.Health += _engine.Config.HealthPerGem;
            _engine.World.Gems += 1;
            _engine.World.Score += _engine.Config.ScorePerGem;
            _engine.RemoveItem(location);
            _engine.Hud.UpdateStatus();
            _engine.PlaySound(2, _engine.Sounds.Gem);

            if (!_engine.Alerts.GemPickup)
                return;

            _engine.SetMessage(0xC8, _engine.Alerts.GemMessage);
            _engine.Alerts.GemPickup = false;
        }
    }
}