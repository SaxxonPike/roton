using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BombBehavior : ElementBehavior
    {
        private readonly IActorList _actorList;
        private readonly IEngine _engine;
        private readonly ISoundSet _soundSet;
        private readonly ITileGrid _tileGrid;
        private readonly IAlerts _alerts;

        public override string KnownName => KnownNames.Bomb;

        public BombBehavior(IActorList actorList, IEngine engine, ISoundSet soundSet, ITileGrid tileGrid, IAlerts alerts)
        {
            _actorList = actorList;
            _engine = engine;
            _soundSet = soundSet;
            _tileGrid = tileGrid;
            _alerts = alerts;
        }
        
        public override void Act(int index)
        {
            var actor = _actorList[index];
            if (actor.P1 <= 0) return;

            actor.P1--;
            _engine.UpdateBoard(actor.Location);
            switch (actor.P1)
            {
                case 1:
                    _engine.PlaySound(1, _soundSet.BombExplode);
                    _engine.UpdateRadius(actor.Location, RadiusMode.Explode);
                    break;
                case 0:
                    var location = actor.Location.Clone();
                    _engine.RemoveActor(index);
                    _engine.UpdateRadius(location, RadiusMode.Clear);
                    break;
                default:
                    _engine.PlaySound(1, (actor.P1 & 0x01) == 0 ? _soundSet.BombTock : _soundSet.BombTick);
                    break;
            }
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            var p1 = _actorList[engine.ActorIndexAt(location)].P1;
            return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, _tileGrid[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var actor = _engine.ActorAt(location);
            if (actor.P1 == 0)
            {
                actor.P1 = 9;
                _engine.UpdateBoard(location);
                _engine.SetMessage(0xC8, _alerts.BombMessage);
                _engine.PlaySound(4, _soundSet.BombActivate);
            }
            else
            {
                _engine.Push(location, vector);
            }
        }
    }
}