using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.ZZT
{
    internal sealed class Core : CoreBase
    {
        public Core(byte[] memoryBytes, byte[] elementBytes)
        {
            Actors = new MemoryActorCollection(Memory);
            BoardData = new MemoryBoard(Memory);
            Serializer = new Serializer(Memory);
            Hud = new TerminalHud(this);
            Elements = new MemoryElementCollection(Memory, elementBytes);
            Sounds = new SoundCollection();
            StateData = new MemoryState(Memory, memoryBytes);
            Tiles = new MemoryTileCollection(Memory);
            WorldData = new MemoryWorld(Memory);
            Grammar = new Grammar(StateData.Colors, Elements);
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