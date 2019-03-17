using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x08)]
    [Context(Context.Super, 0x08)]
    public sealed class KeyInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public KeyInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = Engine.Tiles[location].Color & 0x07;
            var keyIndex = color - 1;
            if (Engine.World.Keys[keyIndex])
            {
                Engine.SetMessage(0xC8, Engine.Alerts.KeyAlreadyMessage(color));
                Engine.PlaySound(2, Engine.Sounds.KeyAlready);
            }
            else
            {
                Engine.World.Keys[keyIndex] = true;
                Engine.RemoveItem(location);
                Engine.Hud.UpdateStatus();
                Engine.SetMessage(0xC8, Engine.Alerts.KeyPickupMessage(color));
                Engine.PlaySound(2, Engine.Sounds.Key);
            }
        }
    }
}