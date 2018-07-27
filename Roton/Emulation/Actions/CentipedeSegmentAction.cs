using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x2D)]
    [ContextEngine(ContextEngine.Super, 0x2D)]
    public sealed class CentipedeSegmentAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public CentipedeSegmentAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            if (actor.Leader < 0)
            {
                if (actor.Leader < -1)
                {
                    Engine.Tiles[actor.Location].Id = Engine.ElementList.HeadId;
                }
                else
                {
                    actor.Leader--;
                }
            }
        }
    }
}