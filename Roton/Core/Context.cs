using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Roton.Emulation.Mapping;
using Roton.Emulation.SuperZZT;
using Roton.Emulation.ZZT;
using Roton.Events;
using Roton.FileIo;

namespace Roton.Core
{
    public sealed class Context : IContext
    {
        private readonly IState _state;
        private readonly IFileSystem _fileSystem;
        private readonly IHud _hud;
        private readonly IDrawer _drawer;
        private readonly IBoards _boards;
        private readonly IActors _actors;
        private readonly IGameSerializer _gameSerializer;
        private readonly IEngine _engine;

        public event EventHandler Terminated;

        private const int MaxGameCycle = 420;

        public Context(
            IActors actors, 
            ITiles tiles, 
            IWorld world,
            IBoards boards,
            IGameSerializer gameSerializer,
            IEngine engine,
            IState state,
            IFileSystem fileSystem,
            IHud hud,
            IDrawer drawer)
        {
            _state = state;
            _fileSystem = fileSystem;
            _hud = hud;
            _drawer = drawer;
            _actors = actors;
            _boards = boards;
            _gameSerializer = gameSerializer;
            _engine = engine;
            Tiles = tiles;
            WorldData = world;
        }

        public void ExecuteOnce()
        {
            if (_state.EditorMode)
            {
                // simulate a game cycle for visuals only
                _state.ActIndex = 0;
                _state.GameCycle++;
                if (_state.GameCycle >= MaxGameCycle)
                {
                    _state.GameCycle = 0;
                }

                foreach (var actor in _actors)
                {
                    if (actor.Cycle > 0 && _state.ActIndex%actor.Cycle == _state.GameCycle%actor.Cycle)
                    {
                        _drawer.UpdateBoard(actor.Location);
                    }
                    _state.ActIndex++;
                }
            }
        }

        public void PackBoard() => _engine.PackBoard();

        public void Refresh() => _hud.RedrawBoard();

        public byte[] Save()
        {
            using (var mem = new MemoryStream())
            {
                var writer = new BinaryWriter(mem);
                _engine.PackBoard();
                writer.Write((short) WorldData.WorldType);
                writer.Write((short) (_boards.Count - 1));
                writer.Flush();
                _gameSerializer.SaveWorld(mem);
                foreach (var board in _boards)
                {
                    _gameSerializer.SaveBoardData(mem, board.Data);
                }
                mem.Flush();
                return mem.ToArray();
            }
        }

        public void Save(string filename)
        {
            _fileSystem.PutFile(filename, Save());
        }

        public void SetBoard(int boardIndex) => _engine.SetBoard(boardIndex);

        public void Start() => _engine.Start();

        public void Stop() => _engine.Stop();

        public ITiles Tiles { get; }

        public void UnpackBoard() => _engine.UnpackBoard(WorldData.BoardIndex);

        public IWorld WorldData { get; }

        public int WorldSize
        {
            get { return _gameSerializer.WorldDataCapacity + _boards.Sum(board => board.Data.Length + 2); }
        }

        private void Initialize(ContextEngine engine)
        {
            _engine.RequestReplaceContext += OnEngineRequestReplaceContext;
            _engine.Terminated += (s, e) => Terminated?.Invoke(s, e);
            _engine.ClearWorld();
        }

        private void OnEngineRequestReplaceContext(object sender, DataEventArgs e)
        {
            using (var mem = new MemoryStream(e.Data))
            {
                DetermineContextEngine(mem);
                LoadAfterType(mem);
            }
        }

        private ContextEngine DetermineContextEngine(Stream stream)
        {
            ContextEngine engine;
            var reader = new BinaryReader(stream);
            switch (reader.ReadInt16())
            {
                case -1:
                    engine = ContextEngine.Zzt;
                    break;
                case -2:
                    engine = ContextEngine.SuperZzt;
                    break;
                default:
                    throw Exceptions.UnknownFormat;
            }
            return engine;
        }

        private void LoadAfterType(Stream stream)
        {
            var reader = new BinaryReader(stream);
            int boardCount = reader.ReadInt16();
            _gameSerializer.LoadWorld(stream);
            _boards.Clear();
            for (var i = 0; i <= boardCount; i++)
            {
                _boards.Add(new PackedBoard(_gameSerializer.LoadBoardData(stream)));
            }
            _gameSerializer.UnpackBoard(Tiles, _boards[WorldData.BoardIndex].Data);
            _state.WorldLoaded = true;
        }
    }
}