using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class GemBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IWorld _world;
        private readonly ISounds _sounds;
        private readonly IEngine _engine;
        private readonly IAlerts _alerts;

        public GemBehavior(IConfig config, IWorld world, ISounds sounds, IEngine engine, IAlerts alerts)
        {
            _config = config;
            _world = world;
            _sounds = sounds;
            _engine = engine;
            _alerts = alerts;
        }

        public override string KnownName => KnownNames.Gem;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _world.Health += _config.HealthPerGem;
            _world.Gems += 1;
            _world.Score += _config.ScorePerGem;
            _engine.RemoveItem(location);
            _engine.UpdateStatus();
            _engine.PlaySound(2, _sounds.Gem);

            if (!_alerts.GemPickup)
                return;

            _engine.SetMessage(0xC8, _alerts.GemMessage);
            _alerts.GemPickup = false;
        }
    }
}