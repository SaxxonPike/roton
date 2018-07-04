using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BombBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IEngine _engine;
        private readonly ISounds _sounds;
        private readonly IGrid _grid;
        private readonly IAlerts _alerts;

        public override string KnownName => KnownNames.Bomb;

        public BombBehavior(IActors actors, IEngine engine, ISounds sounds, IGrid grid, IAlerts alerts)
        {
            _actors = actors;
            _engine = engine;
            _sounds = sounds;
            _grid = grid;
            _alerts = alerts;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];
            if (actor.P1 <= 0) return;

            actor.P1--;
            _engine.UpdateBoard(actor.Location);
            switch (actor.P1)
            {
                case 1:
                    _engine.PlaySound(1, _sounds.BombExplode);
                    _engine.UpdateRadius(actor.Location, RadiusMode.Explode);
                    break;
                case 0:
                    var location = actor.Location.Clone();
                    _engine.RemoveActor(index);
                    _engine.UpdateRadius(location, RadiusMode.Clear);
                    break;
                default:
                    _engine.PlaySound(1, (actor.P1 & 0x01) == 0 ? _sounds.BombTock : _sounds.BombTick);
                    break;
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var p1 = _actors.ActorAt(location).P1;
            return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, _grid[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var actor = _actors.ActorAt(location);
            if (actor.P1 == 0)
            {
                actor.P1 = 9;
                _engine.UpdateBoard(location);
                _engine.SetMessage(0xC8, _alerts.BombMessage);
                _engine.PlaySound(4, _sounds.BombActivate);
            }
            else
            {
                _engine.Push(location, vector);
            }
        }
    }
}