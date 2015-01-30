using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal partial class Core : CoreBase
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

        public override MemoryActorCollectionBase Actors
        {
            get { return _actors; }
        }

        public override MemoryBoardBase BoardData
        {
            get { return _board; }
        }

        public override SerializerBase Disk
        {
            get { return _disk; }
        }

        public override Display Display
        {
            get { return _display; }
        }

        public override MemoryElementCollectionBase Elements
        {
            get { return _elements; }
        }

        public override SoundsBase Sounds
        {
            get { return _sounds; }
        }

        public override MemoryStateBase StateData
        {
            get { return _state; }
        }

        public override bool StonesEnabled
        {
            get { return false; }
        }

        public override MemoryTileCollectionBase Tiles
        {
            get { return _tiles; }
        }

        public override bool TorchesEnabled
        {
            get { return true; }
        }

        public override MemoryWorldBase WorldData
        {
            get { return _world; }
        }
    }
}
