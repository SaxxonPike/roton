using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class SuperZztEngine
    {
        public override void EnterBoard()
        {
            BroadcastLabel(0, @"ENTER", false);
            base.EnterBoard();
        }

        public override void ForcePlayerColor(int index)
        {
            // Do nothing to override the player's color in Super ZZT
        }

        public override void RemoveItem(IXyPair location)
        {
            var result = new Tile(Elements.FloorElement.Id, 0x00);
            var finished = false;

            for (var i = 0; i < 4; i++)
            {
                var targetVector = GetCardinalVector(i);
                var targetLocation = new Location(location.X + targetVector.X, location.Y + targetVector.Y);
                var adjacentTile = TileAt(targetLocation);
                if (Elements[adjacentTile.Id].Cycle >= 0)
                    adjacentTile = this.ActorAt(targetLocation).UnderTile;
                var adjacentElement = adjacentTile.Id;

                if (adjacentElement == Elements.EmptyId ||
                    adjacentElement == Elements.SliderEwId ||
                    adjacentElement == Elements.SliderNsId ||
                    adjacentElement == Elements.BoulderId)
                {
                    finished = true;
                    result.Color = 0;
                }

                if (adjacentElement == Elements.FloorId)
                {
                    result.Color = adjacentTile.Color;
                }

                if (finished)
                {
                    break;
                }
            }

            if (result.Color == 0)
            {
                result.Id = Elements.EmptyId;
            }
            TileAt(location).CopyFrom(result);
        }

        public override void ShowInGameHelp()
        {
            // Super ZZT doesn't have in-game help, but it does have hints
            BroadcastLabel(0, @"HINT", false);
        }
    }
}