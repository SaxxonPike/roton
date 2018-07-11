using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Zzt, 0x2D)]
    [ContextEngine(ContextEngine.SuperZzt, 0x2D)]
    public sealed class CentipedeSegmentAction : IAction
    {
        private readonly IEngine _engine;

        public CentipedeSegmentAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            if (actor.Leader < 0)
            {
                if (actor.Leader < -1)
                {
                    _engine.Tiles[actor.Location].Id = _engine.ElementList.HeadId;
                }
                else
                {
                    actor.Leader--;
                }
            }
        }
    }
}