using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt, 0x0A)]
    [ContextEngine(ContextEngine.SuperZzt, 0x0A)]
    public sealed class ScrollInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public ScrollInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = _engine.ActorIndexAt(location);
            var actor = _engine.Actors[scrollIndex];

            _engine.PlaySound(2, _engine.EncodeMusic(_engine.Config.ScrollMusic));
            _engine.ExecuteCode(scrollIndex, actor, _engine.Config.ScrollTitle);
            _engine.RemoveActor(scrollIndex);
        }
    }
}