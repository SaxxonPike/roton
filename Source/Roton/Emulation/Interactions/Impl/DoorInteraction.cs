using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x09)]
    [Context(Context.Super, 0x09)]
    public sealed class DoorInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public DoorInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = (Engine.Tiles[location].Color & 0x70) >> 4;
            var keyIndex = color - 1;
            if (!Engine.World.Keys[keyIndex])
            {
                Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.DoorLockedMessage(color));
                Engine.PlaySound(3, Engine.Sounds.DoorLocked);
            }
            else
            {
                Engine.World.Keys[keyIndex] = false;
                Engine.RemoveItem(location);
                Engine.Hud.UpdateStatus();
                Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.DoorOpenMessage(color));
                Engine.PlaySound(3, Engine.Sounds.DoorOpen);
            }
        }
    }
}