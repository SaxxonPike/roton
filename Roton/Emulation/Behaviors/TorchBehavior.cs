﻿using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.SuperZZT;

namespace Roton.Emulation.Behaviors
{
    public sealed class TorchBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Torch;

        public TorchBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.World.Torches++;
            _engine.RemoveItem(location);
            _engine.Hud.UpdateStatus();
            if (_engine.Alerts.TorchPickup)
            {
                _engine.SetMessage(0xC8, _engine.Alerts.TorchMessage);
                _engine.Alerts.TorchPickup = false;
            }

            _engine.PlaySound(3, _engine.Sounds.Torch);
        }
    }
}