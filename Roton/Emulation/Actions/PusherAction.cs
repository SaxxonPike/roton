using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Zzt, 0x28)]
    [ContextEngine(ContextEngine.SuperZzt, 0x28)]
    public sealed class PusherAction : IAction
    {
        private readonly IEngine _engine;

        public PusherAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            var source = actor.Location.Clone();

            if (!_engine.Tiles.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor)
            {
                _engine.Push(actor.Location.Sum(actor.Vector), actor.Vector);
            }

            index = _engine.Actors.ActorIndexAt(source);
            actor = _engine.Actors[index];
            
            if (!_engine.Tiles.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor) 
                return;

            _engine.MoveActor(index, actor.Location.Sum(actor.Vector));
            _engine.PlaySound(2, _engine.Sounds.Push);
            var behindLocation = actor.Location.Difference(actor.Vector);
            
            if (_engine.Tiles[behindLocation].Id != _engine.ElementList.PusherId) 
                return;

            var behindIndex = _engine.Actors.ActorIndexAt(behindLocation);
            var behindActor = _engine.Actors[behindIndex];
            if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
            {
                _engine.ActionList.Get(_engine.ElementList.PusherId).Act(behindIndex);
            }
        }
    }
}