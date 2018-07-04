using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class ForestBehavior : ElementBehavior
    {
        private readonly bool _clearToFloor;

        public ForestBehavior(bool clearToFloor)
        {
            _clearToFloor = clearToFloor;
        }

        public override string KnownName => "Forest";

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            if (_clearToFloor)
                engine.Tiles[location].SetTo(engine.Elements.FloorId, 0x02);
            else
                engine.RemoveItem(location);

            engine.UpdateBoard(location);

            var forestIndex = engine.State.ForestIndex;
            var forestSongLength = engine.SoundSet.Forest.Length;
            engine.State.ForestIndex = (forestIndex + 2)%forestSongLength;
            engine.PlaySound(3, engine.SoundSet.Forest, forestIndex, 2);

            if (!engine.Alerts.Forest)
                return;

            engine.SetMessage(0xC8, engine.Alerts.ForestMessage);
            engine.Alerts.Forest = false;
        }
    }
}