using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class DragonPupBehavior : EnemyBehavior
    {
        private readonly IState _state;
        private readonly ITiles _tiles;
        private readonly IActors _actors;
        private readonly IDrawer _drawer;

        public override string KnownName => KnownNames.DragonPup;

        public DragonPupBehavior(IState state, ITiles tiles, IActors actors, IDrawer drawer, IMover mover) : base(mover)
        {
            _state = state;
            _tiles = tiles;
            _actors = actors;
            _drawer = drawer;
        }

        public override void Act(int index)
        {
            _drawer.UpdateBoard(_actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch (_state.GameCycle & 0x3)
            {
                case 0:
                case 2:
                    return new AnsiChar(0x94, _tiles[location].Color);
                case 1:
                    return new AnsiChar(0xA2, _tiles[location].Color);
                default:
                    return new AnsiChar(0x95, _tiles[location].Color);
            }
        }
    }
}