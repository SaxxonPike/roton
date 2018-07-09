using Roton.Emulation.Actions;
using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public class CentipedeSegmentAction : IAction
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