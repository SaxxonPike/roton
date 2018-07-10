using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Actions
{
    public class BearAction : IAction
    {
        private readonly IEngine _engine;

        public BearAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var vector = new Vector();

            if (_engine.Player.Location.X == actor.Location.X ||
                (8 - actor.P1 < _engine.Player.Location.Y.AbsDiff(actor.Location.Y)))
            {
                if (8 - actor.P1 < _engine.Player.Location.X.AbsDiff(actor.Location.X))
                {
                    vector.SetTo(0, 0);
                }
                else
                {
                    vector.SetTo(0, (_engine.Player.Location.Y - actor.Location.Y).Polarity());
                }
            }
            else
            {
                vector.SetTo((_engine.Player.Location.X - actor.Location.X).Polarity(), 0);
            }

            var target = actor.Location.Sum(vector);
            var targetElement = _engine.Tiles.ElementAt(target);

            if (targetElement.IsFloor)
            {
                _engine.MoveActor(index, target);
            }
            else if (targetElement.Id == _engine.ElementList.PlayerId || targetElement.Id == _engine.ElementList.BreakableId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}