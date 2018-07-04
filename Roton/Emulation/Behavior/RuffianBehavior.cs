namespace Roton.Emulation.Behavior
{
    public sealed class RuffianBehavior : EnemyBehavior
    {
        public override string KnownName => "Ruffian";

        public override void Act(int index)
        {
            var actor = _actorList[index];

            if (actor.Vector.IsZero())
            {
                if (actor.P2 + 8 <= engine.SyncRandomNumber(17))
                {
                    if (actor.P1 >= engine.SyncRandomNumber(9))
                    {
                        actor.Vector.CopyFrom(engine.Seek(actor.Location));
                    }
                    else
                    {
                        actor.Vector.CopyFrom(engine.Rnd());
                    }
                }
            }
            else
            {
                if (actor.Location.X == _actorList.GetPlayer().Location.X || actor.Location.Y == _actorList.GetPlayer().Location.Y)
                {
                    if (actor.P1 >= engine.SyncRandomNumber(9))
                    {
                        actor.Vector.CopyFrom(engine.Seek(actor.Location));
                    }
                }

                var target = actor.Location.Sum(actor.Vector);
                if (engine.ElementAt(target).Id == engine.Elements.PlayerId)
                {
                    engine.Attack(index, target);
                }
                else if (engine.ElementAt(target).IsFloor)
                {
                    engine.MoveActor(index, target);
                    if (actor.P2 + 8 <= engine.SyncRandomNumber(17))
                    {
                        actor.Vector.SetTo(0, 0);
                    }
                }
                else
                {
                    actor.Vector.SetTo(0, 0);
                }
            }
        }
    }
}