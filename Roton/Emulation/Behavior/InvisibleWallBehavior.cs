using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class InvisibleWallBehavior : ElementBehavior
    {
        private readonly ITiles _tiles;
        private readonly ISounds _sounds;
        private readonly IElements _elements;
        private readonly IAlerts _alerts;
        private readonly ISounder _sounder;
        private readonly IDrawer _drawer;
        private readonly IMessager _messager;

        public override string KnownName => KnownNames.Invisible;

        public InvisibleWallBehavior(ITiles tiles, ISounds sounds, IElements elements, IAlerts alerts,
            ISounder sounder, IDrawer drawer, IMessager messager)
        {
            _tiles = tiles;
            _sounds = sounds;
            _elements = elements;
            _alerts = alerts;
            _sounder = sounder;
            _drawer = drawer;
            _messager = messager;
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _tiles[location].Id = _elements.NormalId;
            _drawer.UpdateBoard(location);
            _sounder.Play(3, _sounds.Invisible);
            _messager.SetMessage(0x64, _alerts.InvisibleMessage);
        }
    }
}