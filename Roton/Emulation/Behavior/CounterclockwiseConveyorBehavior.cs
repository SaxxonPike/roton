using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class CounterclockwiseConveyorBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly IGrid _grid;
        private readonly IElements _elements;
        private readonly IEngine _engine;
        public override string KnownName => KnownNames.Counter;

        public CounterclockwiseConveyorBehavior(IActors actors, IState state, IGrid grid, IElements elements,
            IEngine engine)
        {
            _actors = actors;
            _state = state;
            _grid = grid;
            _elements = elements;
            _engine = engine;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            _engine.UpdateBoard(actor.Location);
            _engine.Convey(actor.Location, -1);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch ((_state.GameCycle/_elements[_elements.CounterId].Cycle) & 0x3)
            {
                case 3:
                    return new AnsiChar(0xB3, _grid[location].Color);
                case 2:
                    return new AnsiChar(0x2F, _grid[location].Color);
                case 1:
                    return new AnsiChar(0xC4, _grid[location].Color);
                default:
                    return new AnsiChar(0x5C, _grid[location].Color);
            }
        }
    }
}