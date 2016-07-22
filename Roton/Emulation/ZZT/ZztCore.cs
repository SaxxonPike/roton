using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztCore : Execution.Core
    {
        public ZztCore(byte[] memoryBytes, byte[] elementBytes)
        {
            Actors = new ZztActorList(Memory);
            BoardData = new ZztBoard(Memory);
            Serializer = new ZztSerializer(Memory);
            Hud = new ZztHud(this);
            Elements = new ZztElementList(Memory, elementBytes);
            Sounds = new Sounds();
            StateData = new ZztState(Memory, memoryBytes);
            Tiles = new ZztTileGrid(Memory);
            WorldData = new ZztWorld(Memory);
            Grammar = new ZztGrammar(StateData.Colors, Elements);
            InitializeElementDelegates();
        }

        public override IActorList Actors { get; }
        public override IBoard BoardData { get; }
        public override IElementList Elements { get; }
        public override IGrammar Grammar { get; }
        public override IHud Hud { get; }
        public override ISerializer Serializer { get; }
        public override ISounds Sounds { get; }
        public override IState StateData { get; }
        public override ITileGrid Tiles { get; }
        public override IWorld WorldData { get; }

        public override bool StonesEnabled => false;
        public override bool TorchesEnabled => true;

        public override void Interact_Ammo(IXyPair location, int index, IXyPair vector)
        {
            Ammo += 5;
            base.Interact_Ammo(location, index, vector);
        }

        public override void Interact_Gem(IXyPair location, int index, IXyPair vector)
        {
            Health += 1;
            base.Interact_Gem(location, index, vector);
        }
    }
}