using Roton.Extensions;

namespace Roton.Core
{
    public class Compass : ICompass
    {
        private readonly IState _state;
        private readonly IRandom _random;
        private readonly IActors _actors;
        private readonly IWorld _world;

        public Compass(IState state, IRandom random, IActors actors, IWorld world)
        {
            _state = state;
            _random = random;
            _actors = actors;
            _world = world;
        }
        
        public IXyPair GetCardinalVector(int index)
        {
            return new Vector(_state.Vector4[index], _state.Vector4[index + 4]);
        }

        public IXyPair GetConveyorVector(int index)
        {
            return new Vector(_state.Vector8[index], _state.Vector8[index + 8]);
        }

        public IXyPair Rnd()
        {
            var result = new Vector();
            Rnd(result);
            return result;
        }

        private void Rnd(IXyPair result)
        {
            result.X = _random.Synced(3) - 1;
            if (result.X == 0)
            {
                result.Y = (_random.Synced(2) << 1) - 1;
            }
            else
            {
                result.Y = 0;
            }
        }

        public IXyPair RndP(IXyPair vector)
        {
            var result = new Vector();
            result.CopyFrom(
                _random.Synced(2) == 0
                    ? vector.Clockwise()
                    : vector.CounterClockwise());
            return result;
        }

        public IXyPair Seek(IXyPair location)
        {
            var result = new Vector();
            if (_random.Synced(2) == 0 || _actors.Player.Location.Y == location.Y)
            {
                result.X = (_actors.Player.Location.X - location.X).Polarity();
            }

            if (result.X == 0)
            {
                result.Y = (_actors.Player.Location.Y - location.Y).Polarity();
            }

            if (_world.EnergyCycles > 0)
            {
                result.SetOpposite();
            }

            return result;
        }

    }
}