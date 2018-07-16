using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalFeatures : IFeatures
    {
        private readonly IEngine _engine;

        public OriginalFeatures(IEngine engine)
        {
            _engine = engine;
        }

        public void LockActor(int index)
        {
            _engine.Actors[index].P2 = 1;
        }

        public void UnlockActor(int index)
        {
            _engine.Actors[index].P2 = 0;
        }

        public bool IsActorLocked(int index)
        {
            return _engine.Actors[index].P2 != 0;
        }

        public void EnterBoard()
        {
            _engine.Board.Entrance.CopyFrom(_engine.Player.Location);
            if (_engine.Board.IsDark && _engine.Alerts.Dark)
            {
                _engine.SetMessage(0xC8, _engine.Alerts.DarkMessage);
                _engine.Alerts.Dark = false;
            }

            _engine.World.TimePassed = 0;
            _engine.UpdateStatus();
        }

        public void ExecuteMessage(IOopContext context)
        {
            if (context.Message.Count == 1)
            {
                _engine.SetMessage(0xC8, new Message(context.Message));
            }
            else
            {
                _engine.State.KeyVector.SetTo(0, 0);
                _engine.Hud.ShowScroll(context.Message);
            }
        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void HandlePlayerInput(IActor actor, int hotkey)
        {
            switch (hotkey)
            {
                case 0x54: // T
                    if (_engine.World.TorchCycles <= 0)
                    {
                        if (_engine.World.Torches <= 0)
                        {
                            if (_engine.Alerts.NoTorches)
                            {
                                _engine.SetMessage(0xC8, _engine.Alerts.NoTorchMessage);
                                _engine.Alerts.NoTorches = false;
                            }
                        }
                        else if (!_engine.Board.IsDark)
                        {
                            if (_engine.Alerts.NotDark)
                            {
                                _engine.SetMessage(0xC8, _engine.Alerts.NotDarkMessage);
                                _engine.Alerts.NotDark = false;
                            }
                        }
                        else
                        {
                            _engine.World.Torches--;
                            _engine.World.TorchCycles = 0xC8;
                            _engine.UpdateRadius(actor.Location, RadiusMode.Update);
                            _engine.Hud.UpdateStatus();
                        }
                    }

                    break;
                case 0x46: // F
                    break;
            }
        }

        public bool CanPutTile(IXyPair location)
        {
            // do not allow #put on the bottom row
            return location.Y < _engine.Tiles.Height;
        }

        public void ClearForest(IXyPair location)
        {
            _engine.RemoveItem(location);
        }

        public void CleanUpPassageMovement()
        {
            _engine.Tiles[_engine.Player.Location].SetTo(_engine.ElementList.EmptyId, 0);
        }

        public void ForcePlayerColor(int index)
        {
            var actor = _engine.Actors[index];
            var playerElement = _engine.ElementList[_engine.ElementList.PlayerId];
            if (_engine.Tiles[actor.Location].Color == playerElement.Color &&
                playerElement.Character == _engine.Facts.PlayerCharacter) 
                return;
            
            playerElement.Character = _engine.Facts.PlayerCharacter;
            _engine.Tiles[actor.Location].Color = playerElement.Color;
            _engine.UpdateBoard(actor.Location);
        }

        public bool HandleTitleInput(int hotkey)
        {
            switch (hotkey)
            {
                case (int)EngineKeyCode.P:
                    return true;
                case (int)EngineKeyCode.W:
                    break;
                case (int)EngineKeyCode.A:
                    break;
                case (int)EngineKeyCode.E:
                    break;
                case (int)EngineKeyCode.S:
                    break;
                case (int)EngineKeyCode.R:
                    break;
                case (int)EngineKeyCode.H:
                    break;
                case (int)EngineKeyCode.QuestionMark:
                    _engine.Hud.EnterCheat();
                    break;
                case (int)EngineKeyCode.Escape:
                case (int)EngineKeyCode.Q:
                    _engine.State.QuitEngine = _engine.Hud.QuitEngineConfirmation();
                    break;
            }

            return false;
        }

        public void RemoveItem(IXyPair location)
        {
            _engine.Tiles[location].Id = _engine.ElementList.EmptyId;
            _engine.UpdateBoard(location);
        }

        public void ShowInGameHelp()
        {
            _engine.ShowHelp("GAME");
        }

        public string GetWorldName(string baseName)
        {
            return $"{baseName}.ZZT";
        }
    }
}