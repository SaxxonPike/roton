using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;
using Roton.Emulation.Serialization;

namespace Roton.Emulation.ZZT
{
    internal sealed partial class Core : CoreBase
    {
        private readonly MemoryActorCollection _actors;
        private readonly MemoryBoard _board;
        private readonly Serializer _disk;
        private readonly TerminalHud _hud;
        private readonly MemoryElementCollection _elements;
        private readonly Sounds _sounds;
        private readonly MemoryState _state;
        private readonly MemoryTileCollection _tiles;
        private readonly MemoryWorld _world;

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
            InitializeDatArchive();
        }

        public override IActorList Actors => _actors;

        public override IBoard BoardData => _board;

        public override IHud Hud => _hud;

        public override IElementList Elements => _elements;

        public override ISerializer Serializer => _disk;

        public override SoundsBase Sounds => _sounds;

        public override IState StateData => _state;

        public override bool StonesEnabled => false;

        public override ITileGrid Tiles => _tiles;

        public override bool TorchesEnabled => true;

        public override IWorld WorldData => _world;
    }
}