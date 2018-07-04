using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;
using Roton.Extensions;
using Roton.FileIo;
using Roton.Resources;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztEngine : Engine
    {
        private readonly IState _state;
        private readonly IActors _actors;
        private readonly IGrid _grid;
        private readonly IElements _elements;
        private readonly IHud _hud;

        protected override bool ActorIsLocked(int index)
        {
            return _actors[index].P3 != 0;
        }

        public override void EnterBoard()
        {
            BroadcastLabel(0, @"ENTER", false);
            base.EnterBoard();
        }

        protected override void ExecuteMessage(IOopContext context)
        {
            switch (context.Message.Count)
            {
                case 1:
                    SetMessage(0xC8, new Message(string.Empty, context.Message[0]));
                    break;
                case 2:
                    SetMessage(0xC8, new Message(context.Message[0], context.Message[1]));
                    break;
                case 0:
                    break;
                default:
                    _state.KeyVector.SetTo(0, 0);
                    _hud.ShowScroll(context.Message);
                    break;
            }
        }

        public override void ForcePlayerColor(int index)
        {
            // Do nothing to override the player's color in Super ZZT
        }

        public override void HandlePlayerInput(IActor actor, int hotkey)
        {
        }

        public override bool HandleTitleInput(int hotkey)
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
                    _state.QuitZzt = _hud.QuitZztConfirmation();
                    break;
            }

            return false;
        }

        protected override string GetWorldName(string baseName)
        {
            return $"{baseName}.SZT";
        }

        public override void RemoveItem(IXyPair location)
        {
            var result = new Tile(_elements.FloorId, 0x00);
            var finished = false;

            for (var i = 0; i < 4; i++)
            {
                var targetVector = GetCardinalVector(i);
                var targetLocation = new Location(location.X + targetVector.X, location.Y + targetVector.Y);
                var adjacentTile = _grid.TileAt(targetLocation);
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

            _grid.TileAt(location).CopyFrom(result);
        }

        public override void ShowInGameHelp()
        {
            // Super ZZT doesn't have in-game help, but it does have hints
            BroadcastLabel(0, @"HINT", false);
        }

        protected override void StartInit()
        {
            _state.GameSpeed = 4;
            _state.DefaultSaveName = "SAVED";
            _state.DefaultBoardName = "TEMP";
            _state.DefaultWorldName = "MONSTER";
            if (!_state.WorldLoaded)
            {
                ClearWorld();
            }

            if (_state.EditorMode)
                SetEditorMode();
            else
                SetGameMode();
        }

        public SuperZztEngine(IKeyboard keyboard, IBoards boards, IFileSystem fileSystem, IState state,
            IOopContextFactory oopContextFactory, IActors actors, IGrid grid, IRandom random, IBoard board,
            IWorld world, ITimers timers, IElements elements, ISounds sounds, IGameSerializer gameSerializer,
            IAlerts alerts, IHud hud, IGrammar grammar) : base(keyboard, boards, fileSystem, state, oopContextFactory,
            actors, grid, random, board, world, timers, elements, sounds, gameSerializer, alerts, hud, grammar)
        {
            _state = state;
            _actors = actors;
            _grid = grid;
            _elements = elements;
            _hud = hud;
            hud.Initialize();
        }
    }
}