using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Behaviors
{
    public sealed class RotonBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Roton;

        public RotonBehavior(IEngine engine) : base(engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];

            actor.P3--;
            if (actor.P3 < -actor.P2 % 10)
            {
                actor.P3 = actor.P2 * 10 + _engine.Random.Synced(10);
            }

            actor.Vector.CopyFrom(_engine.Seek(actor.Location));
            if (actor.P1 <= _engine.Random.Synced(10))
            {
                var temp = actor.Vector.X;
                actor.Vector.X = -actor.P2.Polarity() * actor.Vector.Y;
                actor.Vector.Y = actor.P2.Polarity() * temp;
            }

            var target = actor.Location.Sum(actor.Vector);
            if (_engine.Tiles.ElementAt(target).IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else if (_engine.Tiles[target].Id == _engine.Elements.PlayerId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}