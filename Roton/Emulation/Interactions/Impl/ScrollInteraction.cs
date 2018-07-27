using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original, 0x0A)]
    [Context(Context.Super, 0x0A)]
    public sealed class ScrollInteraction : IInteraction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public ScrollInteraction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = Engine.ActorIndexAt(location);
            var actor = Engine.Actors[scrollIndex];

            Engine.PlaySound(2, Engine.EncodeMusic("c-c+d-d+e-e+f-f+g-g"));
            Engine.ExecuteCode(scrollIndex, actor, "Scroll");
            Engine.RemoveActor(scrollIndex);
        }
    }
}