using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public class LionBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Lion;

        public LionBehavior(IEngine engine) : base(engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 >= _engine.Random.Synced(10)
                ? _engine.Seek(actor.Location)
                : _engine.Rnd());

            var target = actor.Location.Sum(vector);
            var element = _engine.Tiles.ElementAt(target);
            if (element.IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else if (element.Id == _engine.Elements.PlayerId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}