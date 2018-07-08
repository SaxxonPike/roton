using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class BombBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Bomb;

        public BombBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            if (actor.P1 <= 0) return;

            actor.P1--;
            _engine.UpdateBoard(actor.Location);
            switch (actor.P1)
            {
                case 1:
                    _engine.PlaySound(1, _engine.Sounds.BombExplode);
                    _engine.UpdateRadius(actor.Location, RadiusMode.Explode);
                    break;
                case 0:
                    var location = actor.Location.Clone();
                    _engine.RemoveActor(index);
                    _engine.UpdateRadius(location, RadiusMode.Clear);
                    break;
                default:
                    _engine.PlaySound(1, (actor.P1 & 0x01) == 0 ? _engine.Sounds.BombTock : _engine.Sounds.BombTick);
                    break;
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var p1 = _engine.Actors.ActorAt(location).P1;
            return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, _engine.Tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var actor = _engine.Actors.ActorAt(location);
            if (actor.P1 == 0)
            {
                actor.P1 = 9;
                _engine.UpdateBoard(location);
                _engine.SetMessage(0xC8, _engine.Alerts.BombMessage);
                _engine.PlaySound(4, _engine.Sounds.BombActivate);
            }
            else
            {
                _engine.Push(location, vector);
            }
        }
    }
}