using Roton.Core;
using Roton.Emulation.Behavior;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztMisc : IMisc
    {
        private readonly IState _state;
        private readonly IHud _hud;
        private readonly IBroadcaster _broadcaster;
        private readonly IPassager _passager;
        private readonly IMessenger _messenger;
        private readonly IElements _elements;
        private readonly ITiles _tiles;
        private readonly IActors _actors;
        private readonly ICompass _compass;
        private readonly IEngine _engine;

        public SuperZztMisc(IState state, IHud hud, IBroadcaster broadcaster, IPassager passager,
            IMessenger messenger, IElements elements, ITiles tiles, IActors actors, ICompass compass, IEngine engine)
        {
            _state = state;
            _hud = hud;
            _broadcaster = broadcaster;
            _passager = passager;
            _messenger = messenger;
            _elements = elements;
            _tiles = tiles;
            _actors = actors;
            _compass = compass;
            _engine = engine;
        }

        public void EnterBoard()
        {
            _broadcaster.BroadcastLabel(0, @"ENTER", false);
            _passager.EnterBoard();
        }

        public void ExecuteMessage(IOopContext context)
        {
            switch (context.Message.Count)
            {
                case 1:
                    _messenger.SetMessage(0xC8, new Message(string.Empty, context.Message[0]));
                    break;
                case 2:
                    _messenger.SetMessage(0xC8, new Message(context.Message[0], context.Message[1]));
                    break;
                case 0:
                    break;
                default:
                    _state.KeyVector.SetTo(0, 0);
                    _hud.ShowScroll(context.Message);
                    break;
            }
        }

        public void Init()
        {
            _state.GameSpeed = 4;
            _state.DefaultSaveName = "SAVED";
            _state.DefaultBoardName = "TEMP";
            _state.DefaultWorldName = "MONSTER";
            if (!_state.WorldLoaded)
            {
                _engine.ClearWorld();
            }

            if (_state.EditorMode)
                _engine.SetEditorMode();
            else
                _engine.SetGameMode();
        }

        public void HandlePlayerInput(IActor actor, int hotkey)
        {
            // TODO: this
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
                    _state.QuitZzt = _hud.QuitZztConfirmation();
                    break;
            }

            return false;
        }

        public void RemoveItem(IXyPair location)
        {
            var result = new Tile(_elements.FloorId, 0x00);
            var finished = false;

            for (var i = 0; i < 4; i++)
            {
                var targetVector = _compass.GetCardinalVector(i);
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

        public void ShowInGameHelp()
        {
            // Super ZZT doesn't have in-game help, but it does have hints
            _broadcaster.BroadcastLabel(0, KnownLabels.Hint, false);
        }

        public string GetWorldName(string baseName)
        {
            return $"{baseName}.SZT";
        }
    }
}