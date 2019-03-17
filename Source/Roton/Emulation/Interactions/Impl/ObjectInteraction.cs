using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x24)]
    [Context(Context.Super, 0x24)]
    public sealed class ObjectInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public ObjectInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var objectIndex = Engine.ActorIndexAt(location);
            Engine.BroadcastLabel(-objectIndex, KnownLabels.Touch, false);
        }
    }
}