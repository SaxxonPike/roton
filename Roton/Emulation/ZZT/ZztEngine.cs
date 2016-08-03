using Roton.Core;
using Roton.Emulation.Behavior;
using Roton.Emulation.Execution;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztEngine : Engine
    {
        public ZztEngine(IEngineConfiguration config, byte[] memoryBytes, byte[] elementBytes) : base(config)
        {
            var behaviorConfig = new BehaviorMapConfiguration
            {
                AmmoPerContainer = 5,
                BuggyPassages = true,
                ForestToFloor = false,
                HealthPerGem = 1,
                MultiMovement = false,
                ScorePerGem = 10
            };

            State = new ZztState(Memory, memoryBytes) {EditorMode = config.EditorMode};

            Actors = new ZztActorList(Memory);
            Board = new ZztBoard(Memory);
            GameSerializer = new ZztGameSerializer(Memory);
            Hud = new ZztHud(this, config.Terminal);
            Elements = new ZztElementList(Memory, elementBytes, behaviorConfig);
            SoundSet = new SoundSet();
            Tiles = new ZztTileGrid(Memory);
            World = new ZztWorld(Memory);
            Grammar = new ZztGrammar(State.Colors, Elements);
            DrumBank = new ZztDrumBank(Memory);

            Hud.Initialize();
        }

        public override IActorList Actors { get; }
        public override IBoard Board { get; }
        public override IDrumBank DrumBank { get; }
        public override IElementList Elements { get; }
        public override IGameSerializer GameSerializer { get; }

        public override IGrammar Grammar { get; }
        public override IHud Hud { get; }
        public override ISoundSet SoundSet { get; }
        public override IState State { get; }
        public override ITileGrid Tiles { get; }
        public override IWorld World { get; }

        public override void HandlePlayerInput(IActor actor, int hotkey)
        {
            switch (hotkey)
            {
                case 0x54: // T
                    if (World.TorchCycles <= 0)
                    {
                        if (World.Torches <= 0)
                        {
                            if (Alerts.NoTorches)
                            {
                                SetMessage(0xC8, Alerts.NoTorchMessage);
                                Alerts.NoTorches = false;
                            }
                        }
                        else if (!Board.IsDark)
                        {
                            if (Alerts.NotDark)
                            {
                                SetMessage(0xC8, Alerts.NotDarkMessage);
                                Alerts.NotDark = false;
                            }
                        }
                        else
                        {
                            World.Torches--;
                            World.TorchCycles = 0xC8;
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
                    State.QuitZzt = Hud.QuitZztConfirmation();
                    break;
            }
            return false;
        }
    }
}