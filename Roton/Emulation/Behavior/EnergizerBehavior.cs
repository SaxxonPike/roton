using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class EnergizerBehavior : ElementBehavior
    {
        public override string KnownName => "Energizer";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.PlaySound(9, engine.Sounds.Energizer);
            engine.RemoveItem(location);
            engine.WorldData.EnergyCycles = 0x4B;
            engine.UpdateStatus();
            engine.UpdateBoard(location);
            if (engine.Alerts.EnergizerPickup)
            {
                engine.Alerts.EnergizerPickup = false;
                engine.SetMessage(0xC8, engine.Alerts.EnergizerMessage);
            }
            engine.BroadcastLabel(0, @"ALL:ENERGIZE", false);
        }
    }
}
