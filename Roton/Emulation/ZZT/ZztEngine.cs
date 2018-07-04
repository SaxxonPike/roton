using Roton.Core;
using Roton.Emulation.Execution;
using Roton.FileIo;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztEngine : Engine
    {
        private readonly IState _state;
        private readonly IBoard _board;
        private readonly IWorld _world;
        private readonly IAlerts _alerts;
        private readonly IHud _hud;

        public override void HandlePlayerInput(IActor actor, int hotkey)
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
                                SetMessage(0xC8, _alerts.NoTorchMessage);
                                _alerts.NoTorches = false;
                            }
                        }
                        else if (!_board.IsDark)
                        {
                            if (_alerts.NotDark)
                            {
                                SetMessage(0xC8, _alerts.NotDarkMessage);
                                _alerts.NotDark = false;
                            }
                        }
                        else
                        {
                            _world.Torches--;
                            _world.TorchCycles = 0xC8;
                            UpdateRadius(actor.Location, RadiusMode.Update);
                            UpdateStatus();
                        }
                    }

                    break;
                case 0x46: // F
                    break;
            }
        }

        public override bool HandleTitleInput(int hotkey)
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

        protected override string GetWorldName(string baseName)
        {
            return $"{baseName}.ZZT";
        }

        public ZztEngine(IKeyboard keyboard, IBoards boards, IFileSystem fileSystem, IState state,
            IOopContextFactory oopContextFactory, IActors actors, IGrid grid, IRandom random, IBoard board,
            IWorld world, ITimer timer, IElements elements, ISounds sounds, IGameSerializer gameSerializer,
            IAlerts alerts, IHud hud, IGrammar grammar) : base(keyboard, boards, fileSystem, state, oopContextFactory,
            actors, grid, random, board, world, timer, elements, sounds, gameSerializer, alerts, hud, grammar)
        {
            _state = state;
            _board = board;
            _world = world;
            _alerts = alerts;
            _hud = hud;
            _hud.Initialize();
        }
    }
}