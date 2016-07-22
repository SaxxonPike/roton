using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class SuperZztCore : Execution.Core
    {
        public SuperZztCore(byte[] memoryBytes, byte[] elementBytes)
        {
            Actors = new SuperZztActorList(Memory);
            BoardData = new SuperZztBoard(Memory);
            Serializer = new SuperZztSerializer(Memory);
            Hud = new SuperZztHud(this);
            Elements = new SuperZztElementList(Memory, elementBytes);
            Sounds = new SuperZztSounds();
            StateData = new SuperZztState(Memory, memoryBytes);
            Tiles = new SuperZztTileGrid(Memory);
            WorldData = new SuperZztWorld(Memory);
            Grammar = new Grammar(new SuperZztColorList(Memory), Elements);
            InitializeElementDelegates();
        }

        public override IActorList Actors { get; }
        public override IBoard BoardData { get; }

        public override IGrammar Grammar { get; }

        public override IHud Hud { get; }

        public override IElementList Elements { get; }

        public override ISerializer Serializer { get; }

        public override ISounds Sounds { get; }

        public override IState StateData { get; }

        public override bool StonesEnabled => true;

        public override ITileGrid Tiles { get; }

        public override bool TorchesEnabled => false;

        public override IWorld WorldData { get; }
    }
}