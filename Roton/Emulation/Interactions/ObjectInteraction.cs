﻿using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Interactions
{
    public class ObjectInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public ObjectInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = _engine.ActorIndexAt(location);
            _engine.BroadcastLabel(-objectIndex, KnownLabels.Touch, false);
        }
    }
}