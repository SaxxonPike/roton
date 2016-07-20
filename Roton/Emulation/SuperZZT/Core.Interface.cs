﻿using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;
using Roton.Emulation.Serialization;

namespace Roton.Emulation.SuperZZT
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
        }

        public override IActorList Actors => _actors;

        public override IBoard BoardData => _board;

        public override IHud Hud => _hud;

        public override IElementList Elements => _elements;

        public override ISerializer Serializer => _disk;

        public override ISounds Sounds => _sounds;

        public override IState StateData => _state;

        public override bool StonesEnabled => true;

        public override ITileGrid Tiles => _tiles;

        public override bool TorchesEnabled => false;

        public override IWorld WorldData => _world;
    }
}