using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public sealed class SharkBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Shark;

        public SharkBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 > _engine.Random.Synced(10)
                ? _engine.Seek(actor.Location)
                : _engine.Rnd());

            var target = actor.Location.Sum(vector);
            var targetElement = _engine.Tiles.ElementAt(target);

            if (targetElement.Id == _engine.Elements.WaterId || targetElement.Id == _engine.Elements.LavaId)
            {
                _engine.MoveActor(index, target);
            }
            else if (targetElement.Id == _engine.Elements.PlayerId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}