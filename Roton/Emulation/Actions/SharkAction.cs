using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Actions
{
    public sealed class SharkAction : IAction
    {
        private readonly IEngine _engine;
        
        public SharkAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var vector = new Vector();

            vector.CopyFrom(actor.P1 > _engine.Random.Synced(10)
                ? _engine.Seek(actor.Location)
                : _engine.Rnd());

            var target = actor.Location.Sum(vector);
            var targetElement = _engine.Tiles.ElementAt(target);

            if (targetElement.Id == _engine.ElementList.WaterId || targetElement.Id == _engine.ElementList.LavaId)
            {
                _engine.MoveActor(index, target);
            }
            else if (targetElement.Id == _engine.ElementList.PlayerId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}