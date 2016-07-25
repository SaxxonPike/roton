using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztEngine : Engine
    {
        public ZztEngine(IEngineConfiguration config, byte[] memoryBytes, byte[] elementBytes) : base(config)
        {
            Actors = new ZztActorList(Memory);
            Board = new ZztBoard(Memory);
            Serializer = new ZztSerializer(Memory);
            Hud = new ZztHud(this, config.Terminal);
            Elements = new ZztElementList(Memory, elementBytes);
            Sounds = new Sounds();
            StateData = new ZztState(Memory, memoryBytes);
            Tiles = new ZztTileGrid(Memory);
            WorldData = new ZztWorld(Memory);
            Grammar = new ZztGrammar(StateData.Colors, Elements);
            InitializeElementDelegates();
        }

        public override IActorList Actors { get; }
        public override IBoard Board { get; }
        public override IElementList Elements { get; }
        public override IGrammar Grammar { get; }
        public override IHud Hud { get; }
        public override ISerializer Serializer { get; }
        public override ISounds Sounds { get; }
        public override IState StateData { get; }
        public override ITileGrid Tiles { get; }
        public override bool TorchesEnabled => true;
        public override IWorld WorldData { get; }
    }
}