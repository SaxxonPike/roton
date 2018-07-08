using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.SuperZZT;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztMisc : IMisc
    {
        private readonly IHud _hud;
        private readonly IState _state;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly IMessenger _messenger;
        private readonly IRadius _radius;
        private readonly IBoard _board;
        private readonly IPassager _passager;

        public ZztMisc(IHud hud, IState state, IWorld world, IAlerts alerts, IMessenger messenger, IRadius radius,
            IBoard board, IPassager passager)
        {
            _hud = hud;
            _state = state;
            _world = world;
            _alerts = alerts;
            _messenger = messenger;
            _radius = radius;
            _board = board;
            _passager = passager;
        }

        public void EnterBoard()
        {
            _passager.EnterBoard();
        }

        public void ExecuteMessage(IOopContext context)
        {
            if (context.Message.Count == 1)
            {
                _messenger.SetMessage(0xC8, new Message(context.Message));
            }
            else
            {
                _state.KeyVector.SetTo(0, 0);
                _hud.ShowScroll(context.Message);
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
                    if (_world.TorchCycles <= 0)
                    {
                        if (_world.Torches <= 0)
                        {
                            if (_alerts.NoTorches)
                            {
                                _messenger.SetMessage(0xC8, _alerts.NoTorchMessage);
                                _alerts.NoTorches = false;
                            }
                        }
                        else if (!_board.IsDark)
                        {
                            if (_alerts.NotDark)
                            {
                                _messenger.SetMessage(0xC8, _alerts.NotDarkMessage);
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