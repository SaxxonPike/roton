using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class CounterclockwiseConveyorBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly ITiles _tiles;
        private readonly IElements _elements;
        private readonly IPlotter _plotter;
        private readonly IDrawer _drawer;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Counter;

        public CounterclockwiseConveyorBehavior(IActors actors, IState state, ITiles tiles, IElements elements,
            IPlotter plotter, IDrawer drawer, IMover mover)
        {
            _actors = actors;
            _state = state;
            _tiles = tiles;
            _elements = elements;
            _plotter = plotter;
            _drawer = drawer;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            _drawer.UpdateBoard(actor.Location);
            _mover.Convey(actor.Location, -1);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch ((_state.GameCycle / _elements[_elements.CounterId].Cycle) & 0x3)
            {
                case 3:
                    return new AnsiChar(0xB3, _tiles[location].Color);
                case 2:
                    return new AnsiChar(0x2F, _tiles[location].Color);
                case 1:
                    return new AnsiChar(0xC4, _tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, _tiles[location].Color);
            }
        }
    }
}