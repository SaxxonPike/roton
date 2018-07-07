using System;
using Roton.Core;
using Roton.Emulation.Behaviors;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztGalaxy : IGalaxy
    {
        private readonly Lazy<IUniverse> _universe;
        private readonly IActors _actors;
        private readonly ITiles _tiles;
        private readonly IElements _elements;
        private readonly IBoard _board;
        private readonly IMessenger _messenger;
        private readonly IAlerts _alerts;
        private readonly IWorld _world;
        private readonly IHud _hud;

        public SuperZztGalaxy(Lazy<IUniverse> universe, IActors actors, ITiles tiles, IElements elements, IBoard board,
            IMessenger messenger, IAlerts alerts, IWorld world, IHud hud)
        {
            _universe = universe;
            _actors = actors;
            _tiles = tiles;
            _elements = elements;
            _board = board;
            _messenger = messenger;
            _alerts = alerts;
            _world = world;
            _hud = hud;
        }

        public void LockActor(int index)
        {
            _actors[index].P3 = 1;
        }

        public void UnlockActor(int index)
        {
            _actors[index].P3 = 0;
        }

        public bool IsActorLocked(int index)
        {
            return _actors[index].P3 != 0;
        }

        public void RemoveItem(IXyPair location)
        {
            var result = new Tile(_elements.FloorId, 0x00);
            var finished = false;

            for (var i = 0; i < 4; i++)
            {
                var targetVector = _universe.Value.GetCardinalVector(i);
                var targetLocation = new Location(location.X + targetVector.X, location.Y + targetVector.Y);
                var adjacentTile = _tiles[targetLocation];
                if (_elements[adjacentTile.Id].Cycle >= 0)
                    adjacentTile = _actors.ActorAt(targetLocation).UnderTile;
                var adjacentElement = adjacentTile.Id;

                if (adjacentElement == _elements.EmptyId ||
                    adjacentElement == _elements.SliderEwId ||
                    adjacentElement == _elements.SliderNsId ||
                    adjacentElement == _elements.BoulderId)
                {
                    finished = true;
                    result.Color = 0;
                }

                if (adjacentElement == _elements.FloorId)
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
                result.Id = _elements.EmptyId;
            }

            _tiles[location].CopyFrom(result);
        }

        public string GetWorldName(string baseName)
        {
            throw new System.NotImplementedException();
        }

        public void EnterBoard()
        {
            _universe.Value.BroadcastLabel(0, @"ENTER", false);
            _board.Entrance.CopyFrom(_actors.Player.Location);
            if (_board.IsDark && _alerts.Dark)
            {
                _messenger.SetMessage(0xC8, _alerts.DarkMessage);
                _alerts.Dark = false;
            }

            _world.TimePassed = 0;
            _hud.UpdateStatus();
        }

        public bool HandleTitleInput(int hotkey)
        {
            throw new System.NotImplementedException();
        }

        public void ShowInGameHelp()
        {
            // Super ZZT doesn't have in-game help, but it does have hints
            _universe.Value.BroadcastLabel(0, KnownLabels.Hint, false);
        }
    }
}