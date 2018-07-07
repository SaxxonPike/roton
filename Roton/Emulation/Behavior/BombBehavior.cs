using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BombBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly ISounds _sounds;
        private readonly ITiles _tiles;
        private readonly IAlerts _alerts;
        private readonly ISounder _sounder;
        private readonly IDrawer _drawer;
        private readonly IRadius _radius;
        private readonly IMessager _messager;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Bomb;

        public BombBehavior(IActors actors, ISounds sounds, ITiles tiles, IAlerts alerts,
            ISounder sounder, IDrawer drawer, IRadius radius, IMessager messager, IMover mover)
        {
            _actors = actors;
            _sounds = sounds;
            _tiles = tiles;
            _alerts = alerts;
            _sounder = sounder;
            _drawer = drawer;
            _radius = radius;
            _messager = messager;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            if (actor.P1 <= 0) return;

            actor.P1--;
            _drawer.UpdateBoard(actor.Location);
            switch (actor.P1)
            {
                case 1:
                    _sounder.Play(1, _sounds.BombExplode);
                    _radius.Update(actor.Location, RadiusMode.Explode);
                    break;
                case 0:
                    var location = actor.Location.Clone();
                    _mover.RemoveActor(index);
                    _radius.Update(location, RadiusMode.Clear);
                    break;
                default:
                    _sounder.Play(1, (actor.P1 & 0x01) == 0 ? _sounds.BombTock : _sounds.BombTick);
                    break;
            }
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var p1 = _actors.ActorAt(location).P1;
            return new AnsiChar(p1 > 1 ? 0x30 + p1 : 0x0B, _tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var actor = _actors.ActorAt(location);
            if (actor.P1 == 0)
            {
                actor.P1 = 9;
                _drawer.UpdateBoard(location);
                _messager.SetMessage(0xC8, _alerts.BombMessage);
                _sounder.Play(4, _sounds.BombActivate);
            }
            else
            {
                _mover.Push(location, vector);
            }
        }
    }
}