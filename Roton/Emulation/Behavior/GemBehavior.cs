using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class GemBehavior : ElementBehavior
    {
        private readonly int _healthPerGem;
        private readonly int _scorePerGem;

        public GemBehavior(int healthPerGem, int scorePerGem)
        {
            _healthPerGem = healthPerGem;
            _scorePerGem = scorePerGem;
        }

        public override string KnownName => "Gem";

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            engine.World.Health += _healthPerGem;
            engine.World.Gems += 1;
            engine.World.Score += _scorePerGem;
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