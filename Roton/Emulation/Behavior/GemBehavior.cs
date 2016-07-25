using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class GemBehavior : ElementBehavior
    {
        private readonly int _healthPerGem;

        public GemBehavior(int healthPerGem)
        {
            _healthPerGem = healthPerGem;
        }

        public override string KnownName => "Gem";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.WorldData.Health += _healthPerGem;
            engine.WorldData.Gems += 1;
            engine.WorldData.Score += 10;
            engine.RemoveItem(location);
            engine.UpdateStatus();
            engine.PlaySound(2, engine.Sounds.Gem);

            if (!engine.Alerts.GemPickup)
                return;

            engine.SetMessage(0xC8, engine.Alerts.GemMessage);
            engine.Alerts.GemPickup = false;
        }
    }
}
