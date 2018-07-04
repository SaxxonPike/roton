using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class ForestBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IElements _elements;
        private readonly IGrid _grid;
        private readonly IEngine _engine;
        private readonly IState _state;
        private readonly ISounds _sounds;
        private readonly IAlerts _alerts;

        public ForestBehavior(IConfig config, IElements elements, IGrid grid, IEngine engine, IState state, ISounds sounds, IAlerts alerts)
        {
            _config = config;
            _elements = elements;
            _grid = grid;
            _engine = engine;
            _state = state;
            _sounds = sounds;
            _alerts = alerts;
        }

        public override string KnownName => KnownNames.Forest;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (_config.ForestToFloor)
                _grid[location].SetTo(_elements.FloorId, 0x02);
            else
                _engine.RemoveItem(location);

            _engine.UpdateBoard(location);

            var forestIndex = _state.ForestIndex;
            var forestSongLength = _sounds.Forest.Length;
            _state.ForestIndex = (forestIndex + 2)%forestSongLength;
            __engine.PlaySound(3, _sounds.Forest, forestIndex, 2);

            if (!_alerts.Forest)
                return;

            _engine.SetMessage(0xC8, _alerts.ForestMessage);
            _alerts.Forest = false;
        }
    }
}