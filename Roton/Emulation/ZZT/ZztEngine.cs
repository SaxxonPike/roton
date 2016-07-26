﻿using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztEngine : Engine
    {
        public ZztEngine(IEngineConfiguration config, byte[] memoryBytes, byte[] elementBytes) : base(config)
        {
            State = new ZztState(Memory, memoryBytes) {EditorMode = config.EditorMode};

            Actors = new ZztActorList(Memory);
            Board = new ZztBoard(Memory);
            GameSerializer = new ZztGameSerializer(Memory);
            Hud = new ZztHud(this, config.Terminal);
            Elements = new ZztElementList(Memory, elementBytes);
            SoundSet = new SoundSet();
            Tiles = new ZztTileGrid(Memory);
            World = new ZztWorld(Memory);
            Grammar = new ZztGrammar(State.Colors, Elements);

            Hud.Initialize();
        }

        public override IActorList Actors { get; }
        public override IBoard Board { get; }
        public override IElementList Elements { get; }
        public override IGrammar Grammar { get; }
        public override IHud Hud { get; }
        public override IGameSerializer GameSerializer { get; }
        public override ISoundSet SoundSet { get; }
        public override IState State { get; }
        public override ITileGrid Tiles { get; }
        public override bool TorchesEnabled => true;
        public override IWorld World { get; }
    }
}