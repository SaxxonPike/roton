namespace Roton.Emulation.SuperZZT
{
    internal sealed partial class Core : CoreBase
    {
        MemoryActorCollection _actors;
        MemoryBoard _board;
        Serializer _disk;
        TerminalDisplay _display;
        MemoryElementCollection _elements;
        Sounds _sounds;
        MemoryState _state;
        MemoryTileCollection _tiles;
        MemoryWorld _world;

        public Core()
        {
            _actors = new MemoryActorCollection(Memory);
            _board = new MemoryBoard(Memory);
            _disk = new Serializer(Memory);
            _display = new TerminalDisplay(this);
            _elements = new MemoryElementCollection(Memory);
            _sounds = new Sounds();
            _state = new MemoryState(Memory);
            _tiles = new MemoryTileCollection(Memory);
            _world = new MemoryWorld(Memory);
            InitializeElementDelegates();
        }

        public override MemoryActorCollectionBase Actors => _actors;

        public override MemoryBoardBase BoardData => _board;

        public override Display Display => _display;

        public override MemoryElementCollectionBase Elements => _elements;

        public override SerializerBase Serializer => _disk;

        public override SoundsBase Sounds => _sounds;

        public override MemoryStateBase StateData => _state;

        public override bool StonesEnabled => true;

        public override MemoryTileCollectionBase Tiles => _tiles;

        public override bool TorchesEnabled => false;

        public override MemoryWorldBase WorldData => _world;
    }
}