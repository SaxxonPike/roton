using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class AmmoBehavior : ElementBehavior
    {
        private readonly int _ammoPerPickup;
        public override string KnownName => "Ammo";

        public AmmoBehavior(int ammoPerPickup)
        {
            _ammoPerPickup = ammoPerPickup;
        }

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.WorldData.Ammo += _ammoPerPickup;
            engine.RemoveItem(location);
            engine.UpdateStatus();
            engine.PlaySound(2, engine.SoundSet.Ammo);
            if (engine.Alerts.AmmoPickup)
            {
                engine.SetMessage(0xC8, engine.Alerts.AmmoMessage);
                engine.Alerts.AmmoPickup = false;
            }
        }
    }
}
