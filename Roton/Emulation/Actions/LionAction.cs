using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Zzt, 0x29)]
    [ContextEngine(ContextEngine.SuperZzt, 0x29)]
    public sealed class LionAction : IAction
    {
        private readonly IEngine _engine;

        public LionAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
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
            else if (element.Id == _engine.ElementList.PlayerId)
            {
                _engine.Attack(index, target);
            }
        }
    }
}