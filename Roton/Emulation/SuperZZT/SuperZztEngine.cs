using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztEngine : Engine
    {
        public SuperZztEngine(IEngineConfiguration config, byte[] memoryBytes, byte[] elementBytes) : base(config)
        {
            State = new SuperZztState(Memory, memoryBytes) {EditorMode = config.EditorMode};

            Actors = new SuperZztActorList(Memory);
            Board = new SuperZztBoard(Memory);
            GameSerializer = new SuperZztGameSerializer(Memory);
            Hud = new SuperZztHud(this, config.Terminal);
            Elements = new SuperZztElementList(Memory, elementBytes);
            SoundSet = new SuperZztSoundSet(Memory);
            Tiles = new SuperZztTileGrid(Memory);
            World = new SuperZztWorld(Memory);
            Grammar = new Grammar(new SuperZztColorList(Memory), Elements);

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

        public override bool TorchesEnabled => false;

        public override IWorld World { get; }

        public override void EnterBoard()
        {
            BroadcastLabel(0, @"ENTER", false);
            base.EnterBoard();
        }

        public override void ForcePlayerColor(int index)
        {
            // Do nothing to override the player's color in Super ZZT
        }

        public override void RemoveItem(IXyPair location)
        {
            var result = new Tile(Elements.FloorId, 0x00);
            var finished = false;

            for (var i = 0; i < 4; i++)
            {
                var targetVector = GetCardinalVector(i);
                var targetLocation = new Location(location.X + targetVector.X, location.Y + targetVector.Y);
                var adjacentTile = TileAt(targetLocation);
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
            TileAt(location).CopyFrom(result);
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