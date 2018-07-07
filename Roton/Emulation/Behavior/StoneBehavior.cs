using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class StoneBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly ITiles _tiles;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly IRandom _random;
        private readonly IHud _hud;
        private readonly IDrawer _drawer;
        private readonly IMover _mover;
        private readonly IMessager _messager;

        public override string KnownName => KnownNames.Stone;

        public StoneBehavior(IActors actors, ITiles tiles, IWorld world, IAlerts alerts, IRandom random, IHud hud,
            IDrawer drawer, IMover mover, IMessager messager)
        {
            _actors = actors;
            _tiles = tiles;
            _world = world;
            _alerts = alerts;
            _random = random;
            _hud = hud;
            _drawer = drawer;
            _mover = mover;
            _messager = messager;
        }

        public override void Act(int index)
        {
            _drawer.UpdateBoard(_actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x41 + _random.NonSynced(0x1A), _tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (_world.Stones < 0)
                _world.Stones = 0;

            _world.Stones++;
            _mover.Destroy(location);
            _hud.UpdateStatus();
            _messager.SetMessage(0xC8, _alerts.StoneMessage);
        }
    }
}