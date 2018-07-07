using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class PusherBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly ITiles _tiles;
        private readonly ISounds _sounds;
        private readonly IElements _elements;
        private readonly ISounder _sounder;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Pusher;

        public PusherBehavior(IActors actors, ITiles tiles, ISounds sounds, IElements elements,
            ISounder sounder, IMover mover)
        {
            _actors = actors;
            _tiles = tiles;
            _sounds = sounds;
            _elements = elements;
            _sounder = sounder;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var source = actor.Location.Clone();

            if (!_tiles.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor)
            {
                _mover.Push(actor.Location.Sum(actor.Vector), actor.Vector);
            }

            index = _actors.ActorIndexAt(source);
            actor = _actors[index];
            if (!_tiles.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor) return;

            _mover.MoveActor(index, actor.Location.Sum(actor.Vector));
            _sounder.Play(2, _sounds.Push);
            var behindLocation = actor.Location.Difference(actor.Vector);
            if (_tiles[behindLocation].Id != _elements.PusherId) return;

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
                    return new AnsiChar(0x10, _tiles[location].Color);
                case -1:
                    return new AnsiChar(0x11, _tiles[location].Color);
                default:
                    return actor.Vector.Y == -1
                        ? new AnsiChar(0x1E, _tiles[location].Color)
                        : new AnsiChar(0x1F, _tiles[location].Color);
            }
        }
    }
}