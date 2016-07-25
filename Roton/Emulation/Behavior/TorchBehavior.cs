using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class TorchBehavior : ElementBehavior
    {
        public override string KnownName => "Torch";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.WorldData.Torches++;
            engine.RemoveItem(location);
            engine.UpdateStatus();
            if (engine.Alerts.TorchPickup)
            {
                engine.SetMessage(0xC8, engine.Alerts.TorchMessage);
                engine.Alerts.TorchPickup = false;
            }
            engine.PlaySound(3, engine.SoundSet.Torch);
        }
    }
}
