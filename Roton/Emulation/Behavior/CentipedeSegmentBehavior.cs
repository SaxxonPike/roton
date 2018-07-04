namespace Roton.Emulation.Behavior
{
    public sealed class CentipedeSegmentBehavior : EnemyBehavior
    {
        public override string KnownName => KnownNames.Segment;

        
        
        public override void Act(int index)
        {
            var actor = _actorList[index];
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