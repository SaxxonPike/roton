using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class MonitorBehavior : ElementBehavior
    {
        public override string KnownName => "Monitor";

        public override void Act(IEngine engine, int index)
        {
            if (engine.StateData.KeyPressed != 0)
            {
                engine.StateData.BreakGameLoop = true;
            }
            engine.MoveActorOnRiver(index);
        }
    }
}
