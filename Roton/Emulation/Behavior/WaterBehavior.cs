using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class WaterBehavior : ElementBehavior
    {
        public override string KnownName => "Water";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.PlaySound(3, engine.Sounds.Water);
            engine.SetMessage(0x64, engine.Alerts.WaterMessage);
        }
    }
}
