using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class Core
    {
        public override void Act_Monitor(int index)
        {
            base.Act_Monitor(index);
            MoveActorOnRiver(index);
        }

        public override void Act_Player(int index)
        {
            base.Act_Player(index);
            MoveActorOnRiver(index);
        }

        internal override void EnterBoard()
        {
            BroadcastLabel(0, @"ENTER", false);
            base.EnterBoard();
        }

        internal override void ForcePlayerColor(int index)
        {
            // Do nothing to override the player's color.
        }

        public override void Interact_Ammo(IXyPair location, int index, IXyPair vector)
        {
            Ammo += 20;
            base.Interact_Ammo(location, index, vector);
        }

        public override void Interact_Forest(IXyPair location, int index, IXyPair vector)
        {
            TileAt(location).SetTo(Elements.FloorId, 0x02);
            UpdateBoard(location);

            // TODO: Implement forest music

            if (AlertForest)
            {
                SetMessage(0xC8, ForestMessage);
                AlertForest = false;
            }
        }

        public override void Interact_Gem(IXyPair location, int index, IXyPair vector)
        {
            Health += 10;
            base.Interact_Gem(location, index, vector);
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
                    adjacentTile = ActorAt(targetLocation).UnderTile;
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
    }
}