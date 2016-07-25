using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class KeyBehavior : ElementBehavior
    {
        public override string KnownName => "Key";

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            var color = engine.Tiles[location].Color & 0x07;
            var keyIndex = color - 1;
            if (engine.WorldData.Keys[keyIndex])
            {
                engine.SetMessage(0xC8, engine.Alerts.KeyAlreadyMessage(color));
                engine.PlaySound(2, engine.SoundSet.KeyAlready);
            }
            else
            {
                engine.WorldData.Keys[keyIndex] = true;
                engine.RemoveItem(location);
                engine.SetMessage(0xC8, engine.Alerts.KeyPickupMessage(color));
                engine.PlaySound(2, engine.SoundSet.Key);
            }
        }
    }
}
