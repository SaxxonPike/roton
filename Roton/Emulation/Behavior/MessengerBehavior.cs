using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class MessengerBehavior : ElementBehavior
    {
        public override string KnownName => "Messenger";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            if (actor.Location.X == 0)
            {
                engine.Hud.DrawMessage(new Message(engine.StateData.Message, engine.StateData.Message2), actor.P2 % 7 + 9);
                actor.P2--;
                if (actor.P2 > 0) return;

                engine.RemoveActor(index);
                engine.StateData.ActIndex--;
                engine.Hud.UpdateBorder();
                engine.StateData.Message = string.Empty;
            }
        }
    }
}
