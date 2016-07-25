using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class ForestBehavior : ElementBehavior
    {
        private readonly bool _clearToFloor;

        public ForestBehavior(bool clearToFloor)
        {
            _clearToFloor = clearToFloor;
        }

        public override string KnownName => "Forest";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            if (_clearToFloor)
                engine.Tiles[location].SetTo(engine.Elements.FloorId, 0x02);
            else
                engine.RemoveItem(location);

            engine.UpdateBoard(location);
            engine.PlaySound(3, engine.Sounds.Forest);

            if (!engine.Alerts.Forest)
                return;

            engine.SetMessage(0xC8, engine.Alerts.ForestMessage);
            engine.Alerts.Forest = false;
        }
    }
}
