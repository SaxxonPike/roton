using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class StoneBehavior : ElementBehavior
    {
        public override string KnownName => "Stone of Power";

        public override void Act(IEngine engine, int index)
        {
            engine.UpdateBoard(engine.Actors[index].Location);
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(0x41 + engine.RandomNumber(0x1A), engine.Tiles[location].Color);
        }

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            if (engine.WorldData.Stones < 0)
            {
                engine.WorldData.Stones = 0;
            }
            engine.WorldData.Stones++;
            engine.Destroy(location);
            engine.UpdateStatus();
            engine.SetMessage(0xC8, engine.Alerts.StoneMessage);
        }
    }
}
