using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class CentipedeSegmentBehavior : EnemyBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly ITiles _tiles;

        public override string KnownName => KnownNames.Segment;

        public CentipedeSegmentBehavior(IActors actors, IElements elements, ITiles tiles, IMover mover) : base(mover)
        {
            _actors = actors;
            _elements = elements;
            _tiles = tiles;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            if (actor.Leader < 0)
            {
                if (actor.Leader < -1)
                {
                    _tiles[actor.Location].Id = _elements.HeadId;
                }
                else
                {
                    actor.Leader--;
                }
            }
        }
    }
}