using Roton.Core;
using Roton.Emulation.Behavior;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztEngine : Engine
    {
        public SuperZztEngine(IEngineConfiguration config, byte[] memoryBytes, byte[] elementBytes) : base(config)
        {
            var behaviorConfig = new BehaviorMapConfiguration
            {
                AmmoPerContainer = 20,
                BuggyPassages = true,
                ForestToFloor = false,
                HealthPerGem = 1,
                MultiMovement = false,
                ScorePerGem = 10
            };

            State = new SuperZztState(Memory, memoryBytes) {EditorMode = config.EditorMode};

            Actors = new SuperZztActorList(Memory);
            Board = new SuperZztBoard(Memory);
            GameSerializer = new SuperZztGameSerializer(Memory);
            Hud = new SuperZztHud(this, config.Terminal);
            Elements = new SuperZztElementList(Memory, elementBytes, behaviorConfig);
            SoundSet = new SuperZztSoundSet(Memory);
            Tiles = new SuperZztTileGrid(Memory);
            World = new SuperZztWorld(Memory);
            Grammar = new SuperZztGrammar(State.Colors, Elements);

            Hud.Initialize();
        }

        public override IActorList Actors { get; }
        public override IBoard Board { get; }

        public override IElementList Elements { get; }

        public override IGameSerializer GameSerializer { get; }

        public override IGrammar Grammar { get; }

        public override IHud Hud { get; }

        public override ISoundSet SoundSet { get; }

        public override IState State { get; }

        public override ITileGrid Tiles { get; }

        public override IWorld World { get; }

        protected override bool ActorIsLocked(int index)
        {
            return Actors[index].P3 != 0;
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
                    context.Engine.State.KeyVector.SetTo(0, 0);
                    Hud.ShowScroll(context.Message);
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
                    State.QuitZzt = Hud.QuitZztConfirmation();
                    break;
            }
            return false;
        }

        public override void RemoveItem(IXyPair location)
        {
            var result = new Tile(Elements.FloorId, 0x00);
            var finished = false;

            for (var i = 0; i < 4; i++)
            {
                var targetVector = GetCardinalVector(i);
                var targetLocation = new Location(location.X + targetVector.X, location.Y + targetVector.Y);
                var adjacentTile = this.TileAt(targetLocation);
                if (Elements[adjacentTile.Id].Cycle >= 0)
                    adjacentTile = this.ActorAt(targetLocation).UnderTile;
                var adjacentElement = adjacentTile.Id;

                if (adjacentElement == Elements.EmptyId ||
                    adjacentElement == Elements.SliderEwId ||
                    adjacentElement == Elements.SliderNsId ||
                    adjacentElement == Elements.BoulderId)
                {
                    finished = true;
                    result.Color = 0;
                }

                if (adjacentElement == Elements.FloorId)
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
                result.Id = Elements.EmptyId;
            }
            this.TileAt(location).CopyFrom(result);
        }

        public override void ShowInGameHelp()
        {
            // Super ZZT doesn't have in-game help, but it does have hints
            BroadcastLabel(0, @"HINT", false);
        }

        protected override void StartMain()
        {
            State.GameSpeed = 4;
            State.DefaultSaveName = "SAVED";
            State.DefaultBoardName = "TEMP";
            State.DefaultWorldName = "MONSTER";
            if (!State.WorldLoaded)
            {
                ClearWorld();
            }

            if (State.EditorMode)
                SetEditorMode();
            else
                SetGameMode();

            TitleScreenLoop();
        }
    }
}