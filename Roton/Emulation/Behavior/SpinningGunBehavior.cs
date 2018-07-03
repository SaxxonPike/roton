using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class SpinningGunBehavior : ElementBehavior
    {
        public override string KnownName => "Spinning Gun";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var firingElement = engine.Elements.BulletId;
            var shot = false;

            engine.UpdateBoard(actor.Location);

            if (actor.P2 >= 0x80)
            {
                firingElement = engine.Elements.StarId;
            }

            if ((actor.P2 & 0x7F) > engine.SyncRandomNumber(9))
            {
                if (actor.P1 >= engine.SyncRandomNumber(9))
                {
                    if (actor.Location.X.AbsDiff(engine.Player.Location.X) <= 2)
                    {
                        shot = engine.SpawnProjectile(firingElement, actor.Location,
                            new Vector(0, (engine.Player.Location.Y - actor.Location.Y).Polarity()), true);
                    }
                    if (!shot && actor.Location.Y.AbsDiff(engine.Player.Location.Y) <= 2)
                    {
                        engine.SpawnProjectile(firingElement, actor.Location,
                            new Vector((engine.Player.Location.X - actor.Location.X).Polarity(), 0), true);
                    }
                }
                else
                {
                    engine.SpawnProjectile(firingElement, actor.Location, engine.Rnd(), true);
                }
            }
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            switch (engine.State.GameCycle & 0x7)
            {
                case 0:
                case 1:
                    return new AnsiChar(0x18, engine.Tiles[location].Color);
                case 2:
                case 3:
                    return new AnsiChar(0x1A, engine.Tiles[location].Color);
                case 4:
                case 5:
                    return new AnsiChar(0x19, engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x1B, engine.Tiles[location].Color);
            }
        }
    }
}