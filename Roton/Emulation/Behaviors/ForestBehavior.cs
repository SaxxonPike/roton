using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.SuperZZT;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class ForestBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public ForestBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override string KnownName => KnownNames.Forest;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (_engine.Config.ForestToFloor)
                _engine.Tiles[location].SetTo(_engine.Elements.FloorId, 0x02);
            else
                _engine.RemoveItem(location);

            _engine.UpdateBoard(location);

            var forestIndex = _engine.State.ForestIndex;
            var forestSongLength = _engine.Sounds.Forest.Length;
            _engine.State.ForestIndex = (forestIndex + 2) % forestSongLength;
            _engine.PlaySound(3, _engine.Sounds.Forest, forestIndex, 2);

            if (!_engine.Alerts.Forest)
                return;

            _engine.SetMessage(0xC8, _engine.Alerts.ForestMessage);
            _engine.Alerts.Forest = false;
        }
    }
}