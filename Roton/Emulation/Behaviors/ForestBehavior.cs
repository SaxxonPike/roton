using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class ForestBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IElements _elements;
        private readonly ITiles _tiles;
        private readonly IState _state;
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;
        private readonly ISounder _sounder;
        private readonly IMessenger _messenger;
        private readonly IDrawer _drawer;
        private readonly IMisc _misc;

        public ForestBehavior(IConfig config, IElements elements, ITiles tiles, IState state,
            ISounds sounds, IAlerts alerts, ISounder sounder, IMessenger messenger, IDrawer drawer,
            IMisc misc)
        {
            _config = config;
            _elements = elements;
            _tiles = tiles;
            _state = state;
            _sounds = sounds;
            _alerts = alerts;
            _sounder = sounder;
            _messenger = messenger;
            _drawer = drawer;
            _misc = misc;
        }

        public override string KnownName => KnownNames.Forest;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (_config.ForestToFloor)
                _tiles[location].SetTo(_elements.FloorId, 0x02);
            else
                _misc.RemoveItem(location);

            _drawer.UpdateBoard(location);

            var forestIndex = _state.ForestIndex;
            var forestSongLength = _sounds.Forest.Length;
            _state.ForestIndex = (forestIndex + 2) % forestSongLength;
            _sounder.PlaySound(3, _sounds.Forest, forestIndex, 2);

            if (!_alerts.Forest)
                return;

            _messenger.SetMessage(0xC8, _alerts.ForestMessage);
            _alerts.Forest = false;
        }
    }
}