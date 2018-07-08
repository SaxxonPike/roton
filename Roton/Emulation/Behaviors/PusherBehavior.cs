using Roton.Core;
using Roton.Emulation.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class PusherBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        public override string KnownName => KnownNames.Pusher;

        public PusherBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
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
            
            if (_engine.Tiles[behindLocation].Id != _engine.Elements.PusherId) 
                return;

            var behindIndex = _engine.Actors.ActorIndexAt(behindLocation);
            var behindActor = _engine.Actors[behindIndex];
            if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
            {
                _engine.Elements[_engine.Elements.PusherId].Act(behindIndex);
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var actor = _engine.ActorAt(location);
            switch (actor.Vector.X)
            {
                case 1:
                    return new AnsiChar(0x10, _engine.Tiles[location].Color);
                case -1:
                    return new AnsiChar(0x11, _engine.Tiles[location].Color);
                default:
                    return actor.Vector.Y == -1
                        ? new AnsiChar(0x1E, _engine.Tiles[location].Color)
                        : new AnsiChar(0x1F, _engine.Tiles[location].Color);
            }
        }
    }
}