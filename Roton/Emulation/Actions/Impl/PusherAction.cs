using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x28)]
    [Context(Context.Super, 0x28)]
    public sealed class PusherAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public PusherAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            var source = actor.Location.Clone();

            if (!Engine.Tiles.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor)
            {
                Engine.Push(actor.Location.Sum(actor.Vector), actor.Vector);
            }

            index = Engine.Actors.ActorIndexAt(source);
            actor = Engine.Actors[index];
            
            if (!Engine.Tiles.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor) 
                return;

            Engine.MoveActor(index, actor.Location.Sum(actor.Vector));
            Engine.PlaySound(2, Engine.Sounds.Push);
            var behindLocation = actor.Location.Difference(actor.Vector);
            
            if (Engine.Tiles[behindLocation].Id != Engine.ElementList.PusherId) 
                return;

            var behindIndex = Engine.Actors.ActorIndexAt(behindLocation);
            var behindActor = Engine.Actors[behindIndex];
            if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
            {
                Engine.ActionList.Get(Engine.ElementList.PusherId).Act(behindIndex);
            }
        }
    }
}