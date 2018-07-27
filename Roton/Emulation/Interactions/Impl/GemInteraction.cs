using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x07)]
    [Context(Context.Super, 0x07)]
    public sealed class GemInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public GemInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            Engine.World.Health += Engine.Facts.HealthPerGem;
            Engine.World.Gems += 1;
            Engine.World.Score += Engine.Facts.ScorePerGem;
            Engine.RemoveItem(location);
            Engine.Hud.UpdateStatus();
            Engine.PlaySound(2, Engine.Sounds.Gem);

            if (!Engine.Alerts.GemPickup)
                return;

            Engine.SetMessage(0xC8, Engine.Alerts.GemMessage);
            Engine.Alerts.GemPickup = false;
        }
    }
}