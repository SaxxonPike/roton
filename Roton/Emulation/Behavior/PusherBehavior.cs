using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class PusherBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IGrid _grid;
        private readonly ISounds _sounds;
        private readonly IElements _elements;
        private readonly IEngine _engine;

        public override string KnownName => KnownNames.Pusher;

        public PusherBehavior(IActors actors, IGrid grid, ISounds sounds, IElements elements, IEngine engine)
        {
            _actors = actors;
            _grid = grid;
            _sounds = sounds;
            _elements = elements;
            _engine = engine;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            var source = actor.Location.Clone();

            if (!_grid.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor)
            {
                _engine.Push(actor.Location.Sum(actor.Vector), actor.Vector);
            }

            index = _actors.ActorIndexAt(source);
            actor = _actors[index];
            if (!_grid.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor) return;

            _engine.MoveActor(index, actor.Location.Sum(actor.Vector));
            _engine.PlaySound(2, _sounds.Push);
            var behindLocation = actor.Location.Difference(actor.Vector);
            if (_grid.TileAt(behindLocation).Id != _elements.PusherId) return;

            var behindIndex = _actors.ActorIndexAt(behindLocation);
            var behindActor = _actors[behindIndex];
            if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
            {
                _elements[_elements.PusherId].Act(behindIndex);
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var actor = _actors.ActorAt(location);
            switch (actor.Vector.X)
            {
                case 1:
                    return new AnsiChar(0x10, _grid[location].Color);
                case -1:
                    return new AnsiChar(0x11, _grid[location].Color);
                default:
                    return actor.Vector.Y == -1
                        ? new AnsiChar(0x1E, _grid[location].Color)
                        : new AnsiChar(0x1F, _grid[location].Color);
            }
        }
    }
}