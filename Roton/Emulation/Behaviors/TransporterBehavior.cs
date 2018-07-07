using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class TransporterBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly ITiles _tiles;
        private readonly IDrawer _drawer;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Transporter;

        public TransporterBehavior(IActors actors, IState state, ITiles tiles, IDrawer drawer, IMover mover)
        {
            _actors = actors;
            _state = state;
            _tiles = tiles;
            _drawer = drawer;
            _mover = mover;
        }
        
        public override void Act(int index)
        {
            _drawer.UpdateBoard(_actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var actor = _actors.ActorAt(location);
            int index;

            if (actor.Vector.X == 0)
            {
                if (actor.Cycle > 0)
                    index = (_state.GameCycle/actor.Cycle) & 0x3;
                else
                    index = 0;
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(_state.TransporterVChars[index], _tiles[location].Color);
            }
            if (actor.Cycle > 0)
                index = (_state.GameCycle/actor.Cycle) & 0x3;
            else
                index = 0;
            index += (actor.Vector.X << 1) + 2;
            return new AnsiChar(_state.TransporterHChars[index], _tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _mover.PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }
    }
}