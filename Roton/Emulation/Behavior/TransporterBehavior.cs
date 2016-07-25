using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class TransporterBehavior : ElementBehavior
    {
        public override string KnownName => "Transporter";

        public override void Act(IEngine engine, int index)
        {
            engine.UpdateBoard(engine.Actors[index].Location);
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            var actor = engine.ActorAt(location);
            int index;

            if (actor.Vector.X == 0)
            {
                if (actor.Cycle > 0)
                    index = (engine.StateData.GameCycle / actor.Cycle) & 0x3;
                else
                    index = 0;
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(engine.StateData.TransporterVChars[index], engine.Tiles[location].Color);
            }
            if (actor.Cycle > 0)
                index = (engine.StateData.GameCycle / actor.Cycle) & 0x3;
            else
                index = 0;
            index += (actor.Vector.X << 1) + 2;
            return new AnsiChar(engine.StateData.TransporterHChars[index], engine.Tiles[location].Color);
        }

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }
    }
}
