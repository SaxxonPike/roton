using Roton.Core;

namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class Core : CoreBase
    {
        private MemoryActorCollection _actors;
        private MemoryBoard _board;
        private Serializer _disk;
        private TerminalHud _hud;
        private MemoryElementCollection _elements;
        private Sounds _sounds;
        private MemoryState _state;
        private MemoryTileCollection _tiles;
        private MemoryWorld _world;

        public Core()
        {
            _actors = new MemoryActorCollection(Memory);
            _board = new MemoryBoard(Memory);
            _disk = new Serializer(Memory);
            _hud = new TerminalHud(this);
            _elements = new MemoryElementCollection(Memory);
            _sounds = new Sounds();
            _state = new MemoryState(Memory);
            _tiles = new MemoryTileCollection(Memory);
            _world = new MemoryWorld(Memory);
            InitializeElementDelegates();
        }

        public override MemoryActorCollectionBase Actors => _actors;

        public override MemoryBoardBase BoardData => _board;

        public override IHud Hud => _hud;

        public override MemoryElementCollectionBase Elements => _elements;

        public override SerializerBase Serializer => _disk;

        public override SoundsBase Sounds => _sounds;

        public override IState StateData => _state;

        public override bool StonesEnabled => true;

        public override MemoryTileCollectionBase Tiles => _tiles;

        public override bool TorchesEnabled => false;

        public override MemoryWorldBase WorldData => _world;
    }
}