using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class SpinningGunBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IEngine _engine;
        private readonly IRandom _random;
        private readonly IState _state;
        private readonly IGrid _grid;
        
        public override string KnownName => KnownNames.SpinningGun;

        public SpinningGunBehavior(IActors actors, IElements elements, IEngine engine, IRandom random, IState state, IGrid grid)
        {
            _actors = actors;
            _elements = elements;
            _engine = engine;
            _random = random;
            _state = state;
            _grid = grid;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            var firingElement = _elements.BulletId;
            var shot = false;

            _engine.UpdateBoard(actor.Location);

            if (actor.P2 >= 0x80)
            {
                firingElement = _elements.StarId;
            }

            if ((actor.P2 & 0x7F) > _random.Synced(9))
            {
                if (actor.P1 >= _random.Synced(9))
                {
                    if (actor.Location.X.AbsDiff(_actors.Player.Location.X) <= 2)
                    {
                        shot = _engine.SpawnProjectile(firingElement, actor.Location,
                            new Vector(0, (_actors.Player.Location.Y - actor.Location.Y).Polarity()), true);
                    }
                    if (!shot && actor.Location.Y.AbsDiff(_actors.Player.Location.Y) <= 2)
                    {
                        _engine.SpawnProjectile(firingElement, actor.Location,
                            new Vector((_actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                    }
                }
                else
                {
                    _engine.SpawnProjectile(firingElement, actor.Location, _engine.Rnd(), true);
                }
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch (_state.GameCycle & 0x7)
            {
                case 0:
                case 1:
                    return new AnsiChar(0x18, _grid[location].Color);
                case 2:
                case 3:
                    return new AnsiChar(0x1A, _grid[location].Color);
                case 4:
                case 5:
                    return new AnsiChar(0x19, _grid[location].Color);
                default:
                    return new AnsiChar(0x1B, _grid[location].Color);
            }
        }
    }
}