using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Roton.Emulation.Mapping;
using Roton.Emulation.SuperZZT;
using Roton.Emulation.ZZT;
using Roton.Events;
using Roton.FileIo;
using Roton.Resources;

namespace Roton.Core
{
    public sealed class Context : IContext
    {
        private readonly IMemory _memory;
        private readonly IState _state;
        private readonly IFileSystem _fileSystem;
        private readonly IBoardList _boardList;
        private readonly IActorList _actorList;
        private readonly IGameSerializer _gameSerializer;
        private readonly IEngine _engine;

        public event EventHandler Terminated;

        private const int MaxGameCycle = 420;

        public Context(
            IActorList actorList, 
            ITileGrid tileGrid, 
            IWorld world,
            IBoardList boardList,
            IGameSerializer gameSerializer,
            IEngine engine,
            IMemory memory,
            IState state,
            IFileSystem fileSystem)
        {
            _memory = memory;
            _state = state;
            _fileSystem = fileSystem;
            _actorList = actorList;
            _boardList = boardList;
            _gameSerializer = gameSerializer;
            _engine = engine;
            Tiles = tileGrid;
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

                foreach (var actor in _actorList)
                {
                    if (actor.Cycle > 0 && _state.ActIndex%actor.Cycle == _state.GameCycle%actor.Cycle)
                    {
                        _engine.UpdateBoard(actor.Location);
                    }
                    _state.ActIndex++;
                }
            }
        }

        public void PackBoard() => _engine.PackBoard();

        public void Refresh() => _engine.RedrawBoard();

        public byte[] Save()
        {
            using (var mem = new MemoryStream())
            {
                var writer = new BinaryWriter(mem);
                _engine.PackBoard();
                writer.Write((short) WorldData.WorldType);
                writer.Write((short) (_boardList.Count - 1));
                writer.Flush();
                _gameSerializer.SaveWorld(mem);
                foreach (var board in _boardList)
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

        public ITileGrid Tiles { get; }

        public void UnpackBoard() => _engine.UnpackBoard(WorldData.BoardIndex);

        public IWorld WorldData { get; }

        public int WorldSize
        {
            get { return _gameSerializer.WorldDataCapacity + _boardList.Sum(board => board.Data.Length + 2); }
        }

        private void Initialize(ContextEngine engine)
        {
//            var resources = ResourceZipFileSystem.System;
//            switch (engine)
//            {
//                case ContextEngine.Zzt:
//                    _engine = new ZztEngine(_config, resources.GetFile("memory-zzt.bin"), resources.GetFile("elements-zzt.bin"));
//                    break;
//                case ContextEngine.SuperZzt:
//                    _engine = new SuperZztEngine(_config, resources.GetFile("memory-szzt.bin"), resources.GetFile("elements-szzt.bin"));
//                    break;
//                default:
//                    throw Exceptions.InvalidFormat;
//            }

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

        private void Initialize(Stream stream)
        {
            var engine = DetermineContextEngine(stream);
            Initialize(engine);
            LoadAfterType(stream);
        }

        private void LoadAfterType(Stream stream)
        {
            var reader = new BinaryReader(stream);
            int boardCount = reader.ReadInt16();
            _gameSerializer.LoadWorld(stream);
            _boardList.Clear();
            for (var i = 0; i <= boardCount; i++)
            {
                _boardList.Add(new PackedBoard(_gameSerializer.LoadBoardData(stream)));
            }
            _gameSerializer.UnpackBoard(Tiles, _boardList[WorldData.BoardIndex].Data);
            _state.WorldLoaded = true;
        }
    }
}