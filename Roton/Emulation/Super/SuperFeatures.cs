using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperFeatures : IFeatures
    {
        private readonly IEngine _engine;

        public SuperFeatures(IEngine engine)
        {
            _engine = engine;
        }

        public void LockActor(int index)
        {
            _engine.Actors[index].P3 = 1;
        }

        public void UnlockActor(int index)
        {
            _engine.Actors[index].P3 = 0;
        }

        public bool IsActorLocked(int index)
        {
            return _engine.Actors[index].P3 != 0;
        }

        public void RemoveItem(IXyPair location)
        {
            var result = new Tile(_engine.ElementList.FloorId, 0x00);
            var finished = false;

            for (var i = 0; i < 4; i++)
            {
                var targetVector = _engine.GetCardinalVector(i);
                var targetLocation = new Location(location.X + targetVector.X, location.Y + targetVector.Y);
                var adjacentTile = _engine.Tiles[targetLocation];
                if (_engine.ElementList[adjacentTile.Id].Cycle >= 0)
                    adjacentTile = _engine.ActorAt(targetLocation).UnderTile;
                var adjacentElement = adjacentTile.Id;

                if (adjacentElement == _engine.ElementList.EmptyId ||
                    adjacentElement == _engine.ElementList.SliderEwId ||
                    adjacentElement == _engine.ElementList.SliderNsId ||
                    adjacentElement == _engine.ElementList.BoulderId)
                {
                    finished = true;
                    result.Color = 0;
                }

                if (adjacentElement == _engine.ElementList.FloorId)
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
                result.Id = _engine.ElementList.EmptyId;
            }

            _engine.Tiles[location].CopyFrom(result);
        }

        public string GetWorldName(string baseName)
        {
            return $"{baseName}.SZT";
        }

        public void EnterBoard()
        {
            _engine.BroadcastLabel(0, KnownLabels.Enter, false);
            _engine.Board.Entrance.CopyFrom(_engine.Actors.Player.Location);
            if (_engine.Board.IsDark && _engine.Alerts.Dark)
            {
                _engine.SetMessage(0xC8, _engine.Alerts.DarkMessage);
                _engine.Alerts.Dark = false;
            }

            _engine.World.TimePassed = 0;
            _engine.Hud.UpdateStatus();
        }

        public bool HandleTitleInput(int hotkey)
        {
            switch (hotkey)
            {
                case 0x0D: // Enter
                    return true;
                case 0x57: // W
                    break;
                case 0x52: // R
                    break;
                case 0x48: // H
                    ShowInGameHelp();
                    break;
                case 0x7C: // ?
                    break;
                case 0x1B: // esc
                case 0x51: // Q
                    _engine.State.QuitEngine = _engine.Hud.QuitEngineConfirmation();
                    break;
            }

            return false;
        }

        public void ShowInGameHelp()
        {
            _engine.BroadcastLabel(0, _engine.Facts.HintLabel, false);
        }

        public void ExecuteMessage(IOopContext context)
        {
            switch (context.Message.Count)
            {
                case 1:
                    _engine.SetMessage(0xC8, new Message(string.Empty, context.Message[0]));
                    break;
                case 2:
                    _engine.SetMessage(0xC8, new Message(context.Message[0], context.Message[1]));
                    break;
                case 0:
                    break;
                default:
                    _engine.State.KeyVector.SetTo(0, 0);
                    _engine.Hud.ShowScroll(context.Message);
                    break;
            }
        }

        public void Init()
        {
            _engine.State.GameSpeed = 4;
            _engine.State.DefaultSaveName = "SAVED";
            _engine.State.DefaultBoardName = "TEMP";
            _engine.State.DefaultWorldName = "MONSTER";
            if (!_engine.State.WorldLoaded)
            {
                _engine.ClearWorld();
            }

            if (_engine.State.EditorMode)
                _engine.SetEditorMode();
            else
                _engine.SetGameMode();
        }

        public void HandlePlayerInput(IActor actor, int hotkey)
        {
            // todo: this
        }

        public bool CanPutTile(IXyPair location)
        {
            return true;
        }

        public void ClearForest(IXyPair location)
        {
            _engine.Tiles[location].SetTo(_engine.ElementList.FloorId, 0x02);
        }

        public void CleanUpPassageMovement()
        {
            _engine.Tiles[_engine.Player.Location].CopyFrom(_engine.Player.UnderTile);
        }

        public void ForcePlayerColor(int index)
        {
            // Super does not enforce player's background color.
        }
    }
}