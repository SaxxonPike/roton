using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.SuperZZT;
using Roton.FileIo;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztMisc : IMisc
    {
        private readonly IHud _hud;
        private readonly IState _state;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly IMessager _messager;
        private readonly IRadius _radius;
        private readonly IBoard _board;

        public ZztMisc(IHud hud, IState state, IWorld world, IAlerts alerts, IMessager messager, IRadius radius, IBoard board)
        {
            _hud = hud;
            _state = state;
            _world = world;
            _alerts = alerts;
            _messager = messager;
            _radius = radius;
            _board = board;
        }
        
        public void EnterBoard()
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteMessage(IOopContext context)
        {
            throw new System.NotImplementedException();
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
                    if (_world.TorchCycles <= 0)
                    {
                        if (_world.Torches <= 0)
                        {
                            if (_alerts.NoTorches)
                            {
                                _messager.SetMessage(0xC8, _alerts.NoTorchMessage);
                                _alerts.NoTorches = false;
                            }
                        }
                        else if (!_board.IsDark)
                        {
                            if (_alerts.NotDark)
                            {
                                _messager.SetMessage(0xC8, _alerts.NotDarkMessage);
                                _alerts.NotDark = false;
                            }
                        }
                        else
                        {
                            _world.Torches--;
                            _world.TorchCycles = 0xC8;
                            _radius.Update(actor.Location, RadiusMode.Update);
                            _hud.UpdateStatus();
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
                    _state.QuitZzt = _hud.QuitZztConfirmation();
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
            throw new System.NotImplementedException();
        }

        public string GetWorldName(string baseName)
        {
            return $"{baseName}.ZZT";
        }
    }
}