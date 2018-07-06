using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class CentipedeSegmentBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IGrid _grid;

        public override string KnownName => KnownNames.Segment;

        public CentipedeSegmentBehavior(IActors actors, IElements elements, IEngine engine, IGrid grid) : base(engine)
        {
            _actors = actors;
            _elements = elements;
            _grid = grid;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            if (actor.Leader < 0)
            {
                if (actor.Leader < -1)
                {
                    _grid.TileAt(actor.Location).Id = _elements.HeadId;
                }
                else
                {
                    actor.Leader--;
                }
            }
        }
    }
}