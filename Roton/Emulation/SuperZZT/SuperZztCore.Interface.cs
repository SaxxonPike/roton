using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class SuperZztEngine : Engine
    {
        public SuperZztEngine(IEngineConfiguration config, byte[] memoryBytes, byte[] elementBytes) : base(config)
        {
            Actors = new SuperZztActorList(Memory);
            Board = new SuperZztBoard(Memory);
            Serializer = new SuperZztSerializer(Memory);
            Hud = new SuperZztHud(this, config.Terminal);
            Elements = new SuperZztElementList(Memory, elementBytes);
            Sounds = new SuperZztSounds();
            StateData = new SuperZztState(Memory, memoryBytes);
            Tiles = new SuperZztTileGrid(Memory);
            WorldData = new SuperZztWorld(Memory);
            Grammar = new Grammar(new SuperZztColorList(Memory), Elements);
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

        public override bool TorchesEnabled => false;

        public override IWorld WorldData { get; }
    }
}