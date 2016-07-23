using System.Collections.Generic;
using System.IO;
using System.Linq;
using Roton.Emulation.Mapping;
using Roton.Emulation.SuperZZT;
using Roton.Emulation.ZZT;
using Roton.Resources;

namespace Roton.Core
{
    public sealed class Context : IContext
    {
        private const int MaxGameCycle = 420;
        private readonly ICoreConfiguration _config;

        public Context(ICoreConfiguration config, byte[] data)
        {
            _config = config;
            using (var mem = new MemoryStream(data))
            {
                Initialize(mem);
            }
        }

        public Context(ICoreConfiguration config, ContextEngine engine)
        {
            _config = config;
            Initialize(engine);
        }

        public Context(ICoreConfiguration config, Stream stream)
        {
            _config = config;
            Initialize(stream);
        }

        private ICore Core { get; set; }
        public int ActorCapacity => Core.Actors.Capacity;

        public IActorList Actors => Core.Actors;

        public int Board
        {
            get { return Core.Board; }
            set { Core.SetBoard(value); }
        }

        public IBoard BoardData => Core.BoardData;

        public IList<IPackedBoard> Boards => Core.Boards;

        public ContextEngine ContextEngine { get; private set; }

        public IActor CreateActor()
        {
            if (Core.ActorCount >= Actors.Capacity - 2)
            {
                return null;
            }
            Core.ActorCount++;
            return Actors[Core.ActorCount];
        }

        public IElementList Elements => Core.Elements;

        public void ExecuteOnce()
        {
            if (Core.EditorMode)
            {
                // simulate a game cycle for visuals only
                Core.ActIndex = 0;
                Core.GameCycle++;
                if (Core.GameCycle >= MaxGameCycle)
                {
                    Core.GameCycle = 0;
                }

                foreach (var actor in Actors)
                {
                    if (actor.Cycle > 0 && Core.ActIndex%actor.Cycle == Core.GameCycle%actor.Cycle)
                    {
                        Core.UpdateBoard(actor.Location);
                    }
                    Core.ActIndex++;
                }
            }
        }

        public int Height => Core.Height;

        public IKeyboard Keyboard
        {
            get { return _config.Keyboard; }
        }

        public byte[] Memory => Core.Memory.Dump();

        public void PackBoard()
        {
            Core.PackBoard();
        }

        public void Refresh()
        {
        }

        public byte[] Save()
        {
            using (var mem = new MemoryStream())
            {
                var writer = new BinaryWriter(mem);
                Core.PackBoard();
                writer.Write((short) WorldData.WorldType);
                writer.Write((short) (Boards.Count - 1));
                writer.Flush();
                Serializer.SaveWorld(mem);
                foreach (var board in Boards)
                {
                    Serializer.SaveBoardData(mem, board.Data);
                }
                mem.Flush();
                return mem.ToArray();
            }
        }

        public void Save(string filename)
        {
            Core.Disk.PutFile(filename, Save());
        }

        public int ScreenHeight { get; private set; }

        public bool ScreenWide { get; private set; }

        public int ScreenWidth { get; private set; }

        public ISerializer Serializer => Core.Serializer;

        public void SetBoard(int boardIndex)
        {
            Core.SetBoard(boardIndex);
        }

        public ISpeaker Speaker => _config.Speaker;

        public void Start()
        {
            Core.Start();
        }

        public void Stop()
        {
            Core.Stop();
        }

        public ITerminal Terminal => Core.Terminal;

        public ITile TileAt(int x, int y)
        {
            return Core.Tiles[new Location(x, y)];
        }

        public void UnpackBoard()
        {
            Core.UnpackBoard(Core.Board);
        }

        public int Width => Core.Width;

        public IWorld WorldData => Core.WorldData;

        public int WorldSize
        {
            get { return Serializer.WorldDataCapacity + Boards.Sum(board => board.Data.Length + 2); }
        }

        private void Initialize(ContextEngine engine)
        {
            var resources = new ResourceZipFileSystem(Properties.Resources.resources);
            ContextEngine = engine;
            switch (engine)
            {
                case ContextEngine.Zzt:
                    Core = new ZztCore(_config, resources.GetZztMemoryData(), resources.GetZztElementData());
                    ScreenWidth = 80;
                    ScreenHeight = 25;
                    ScreenWide = false;
                    break;
                case ContextEngine.SuperZzt:
                    Core = new SuperZztCore(_config, resources.GetSuperZztMemoryData(),
                        resources.GetSuperZztElementData());
                    ScreenWidth = 40;
                    ScreenHeight = 25;
                    ScreenWide = true;
                    break;
                default:
                    throw Exceptions.InvalidFormat;
            }

            if (_config.EditorMode)
            {
                // editor mode will always show the full board
                ScreenWidth = Width;
                ScreenHeight = Height;
            }

            Core.ClearWorld();
            Core.EditorMode = _config.EditorMode;
        }

        private void Initialize(Stream stream)
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
            Initialize(engine);
            LoadAfterType(stream);
        }

        internal void Load(Stream stream)
        {
            var reader = new BinaryReader(stream);
            if (reader.ReadInt16() != WorldData.WorldType)
            {
                throw Exceptions.InvalidFormat;
            }
            LoadAfterType(stream);
        }

        private void LoadAfterType(Stream stream)
        {
            var reader = new BinaryReader(stream);
            int boardCount = reader.ReadInt16();
            Serializer.LoadWorld(stream);
            Boards.Clear();
            for (var i = 0; i <= boardCount; i++)
            {
                Boards.Add(new PackedBoard(Serializer.LoadBoardData(stream)));
            }
            Serializer.UnpackBoard(Core.Tiles, Core.Boards[Core.Board].Data);
            Core.WorldLoaded = true;
        }
    }
}