using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Zzt
{
    public class ZztFeatures : IFeatures
    {
        private readonly IEngine _engine;

        public ZztFeatures(IEngine engine)
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
            throw new System.NotImplementedException();
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

        public bool HandleTitleInput(int hotkey)
        {
            switch (hotkey)
            {
                case 0x50: // P
                    return true;
                case 0x57: // W
                    break;
                case 0x41: // A
                    break;
                case 0x45: // E
                    break;
                case 0x53: // S
                    break;
                case 0x52: // R
                    break;
                case 0x48: // H
                    break;
                case 0x7C: // ?
                    break;
                case 0x1B: // esc
                case 0x51: // Q
                    _engine.State.QuitZzt = _engine.Hud.QuitZztConfirmation();
                    break;
            }

            return false;
        }

        public void RemoveItem(IXyPair location)
        {
            throw new System.NotImplementedException();
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