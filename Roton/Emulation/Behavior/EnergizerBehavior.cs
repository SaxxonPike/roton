﻿using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class EnergizerBehavior : ElementBehavior
    {
        public override string KnownName => "Energizer";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.PlaySound(9, engine.SoundSet.Energizer);
            engine.RemoveItem(location);
            engine.World.EnergyCycles = 0x4B;
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