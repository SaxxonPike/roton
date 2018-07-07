using System;
using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Zzt
{
    public class ZztGalaxy : IGalaxy
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
        private readonly IState _state;

        public ZztGalaxy(Lazy<IUniverse> universe, IActors actors, ITiles tiles, IElements elements, IBoard board,
            IMessenger messenger, IAlerts alerts, IWorld world, IHud hud, IState state)
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
            _state = state;
        }

        public void LockActor(int index)
        {
            _actors[index].P2 = 1;
        }

        public void UnlockActor(int index)
        {
            _actors[index].P2 = 0;
        }

        public bool IsActorLocked(int index)
        {
            return _actors[index].P2 != 0;
        }

        public void EnterBoard()
        {
            throw new System.NotImplementedException();
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
                            _universe.Value.Radius(actor.Location, RadiusMode.Update);
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