using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class CentipedeSegmentBehavior : EnemyBehavior
    {
        public override string KnownName => "Centipede (Segment)";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            if (actor.Leader < 0)
            {
                if (actor.Leader < -1)
                {
                    engine.TileAt(actor.Location).Id = engine.Elements.HeadId;
                }
                else
                {
                    actor.Leader--;
                }
            }
        }
    }
}