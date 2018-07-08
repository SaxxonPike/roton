using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class CentipedeSegmentBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;

        public override string KnownName => KnownNames.Segment;

        public CentipedeSegmentBehavior(IEngine engine) : base(engine)
        {
            _engine = engine;
        }
        
        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            if (actor.Leader < 0)
            {
                if (actor.Leader < -1)
                {
                    _engine.Tiles[actor.Location].Id = _engine.Elements.HeadId;
                }
                else
                {
                    actor.Leader--;
                }
            }
        }
    }
}