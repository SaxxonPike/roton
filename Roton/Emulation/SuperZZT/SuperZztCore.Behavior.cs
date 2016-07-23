using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class SuperZztCore
    {
        public override void ActMonitor(int index)
        {
            base.ActMonitor(index);
            MoveActorOnRiver(index);
        }

        public override void ActPlayer(int index)
        {
            base.ActPlayer(index);
            MoveActorOnRiver(index);
        }

        protected override void EnterBoard()
        {
            BroadcastLabel(0, @"ENTER", false);
            base.EnterBoard();
        }

        protected override void ExecutePassageCleanup()
        {
            // Passage holes were fixed in Super ZZT
            TileAt(Player.Location).CopyFrom(Player.UnderTile);
        }

        protected override void ForcePlayerColor(int index)
        {
            // Do nothing to override the player's color in Super ZZT
        }

        public override void InteractAmmo(IXyPair location, int index, IXyPair vector)
        {
            Ammo += 20;
            base.InteractAmmo(location, index, vector);
        }

        public override void InteractForest(IXyPair location, int index, IXyPair vector)
        {
            TileAt(location).SetTo(Elements.FloorId, 0x02);
            UpdateBoard(location);

            // TODO: Implement forest music

            if (Alerts.AlertForest)
            {
                SetMessage(0xC8, ForestMessage);
                Alerts.AlertForest = false;
            }
        }

        public override void InteractGem(IXyPair location, int index, IXyPair vector)
        {
            Health += 10;
            base.InteractGem(location, index, vector);
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

        protected override void ShowInGameHelp()
        {
            // Super ZZT doesn't have in-game help, but it does have hints
            BroadcastLabel(0, @"HINT", false);
        }
    }
}