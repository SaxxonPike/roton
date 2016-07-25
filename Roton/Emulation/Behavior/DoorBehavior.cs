using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class DoorBehavior : ElementBehavior
    {
        public override string KnownName => "Door";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            var color = (engine.TileAt(location).Color & 0x70) >> 4;
            var keyIndex = color - 1;
            if (!engine.WorldData.Keys[keyIndex])
            {
                engine.SetMessage(0xC8, engine.Alerts.DoorLockedMessage(color));
                engine.PlaySound(3, engine.Sounds.DoorLocked);
            }
            else
            {
                engine.WorldData.Keys[keyIndex] = false;
                engine.RemoveItem(location);
                engine.SetMessage(0xC8, engine.Alerts.DoorOpenMessage(color));
                engine.PlaySound(3, engine.Sounds.DoorOpen);
            }
        }
    }
}
