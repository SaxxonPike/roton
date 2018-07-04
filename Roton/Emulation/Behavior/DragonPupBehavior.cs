using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class DragonPupBehavior : EnemyBehavior
    {
        private readonly IEngine _engine;
        private readonly IState _state;
        private readonly IGrid _grid;
        private readonly IActors _actors;

        public override string KnownName => KnownNames.DragonPup;

        public DragonPupBehavior(IEngine engine, IState state, IGrid grid, IActors actors) : base(engine)
        {
            _engine = engine;
            _state = state;
            _grid = grid;
            _actors = actors;
        }
        
        public override void Act(int index)
        {
            _engine.UpdateBoard(_actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch (_state.GameCycle & 0x3)
            {
                case 0:
                case 2:
                    return new AnsiChar(0x94, _grid[location].Color);
                case 1:
                    return new AnsiChar(0xA2, _grid[location].Color);
                default:
                    return new AnsiChar(0x95, _grid[location].Color);
            }
        }
    }
}