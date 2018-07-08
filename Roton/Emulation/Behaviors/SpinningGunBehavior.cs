using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public sealed class SpinningGunBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.SpinningGun;

        public SpinningGunBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            var firingElement = _engine.Elements.BulletId;
            var shot = false;

            _engine.UpdateBoard(actor.Location);

            if (actor.P2 >= 0x80)
            {
                firingElement = _engine.Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > _engine.Random.Synced(9))
            {
                if (actor.P1 >= _engine.Random.Synced(9))
                {
                    if (actor.Location.X.AbsDiff(_engine.Actors.Player.Location.X) <= 2)
                    {
                        shot = _engine.SpawnProjectile(firingElement, actor.Location,
                            new Vector(0, (_engine.Actors.Player.Location.Y - actor.Location.Y).Polarity()), true);
                    }

                    if (!shot && actor.Location.Y.AbsDiff(_engine.Actors.Player.Location.Y) <= 2)
                    {
                        _engine.SpawnProjectile(firingElement, actor.Location,
                            new Vector((_engine.Actors.Player.Location.X - actor.Location.X).Polarity(), 0), true);
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
            switch (_engine.State.GameCycle & 0x7)
            {
                case 0:
                case 1:
                    return new AnsiChar(0x18, _engine.Tiles[location].Color);
                case 2:
                case 3:
                    return new AnsiChar(0x1A, _engine.Tiles[location].Color);
                case 4:
                case 5:
                    return new AnsiChar(0x19, _engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x1B, _engine.Tiles[location].Color);
            }
        }
    }
}