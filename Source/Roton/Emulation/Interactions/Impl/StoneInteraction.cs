using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Super, 0x40)]
    public sealed class StoneInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;
        
        public StoneInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (Engine.World.Stones < 0)
                Engine.World.Stones = 0;

            Engine.World.Stones++;
            Engine.Destroy(location);
            Engine.Hud.UpdateStatus();
            Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.StoneMessage);
        }
    }
}