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
        private readonly IEngineConfiguration _config;

        public Context(IEngineConfiguration config, byte[] data)
        {
            _config = config;
            using (var mem = new MemoryStream(data))
            {
                Initialize(mem);
            }
        }

        public Context(IEngineConfiguration config, ContextEngine engine)
        {
            _config = config;
            Initialize(engine);
        }

        public Context(IEngineConfiguration config, Stream stream)
        {
            _config = config;
            Initialize(stream);
        }

        private IGameSerializer GameSerializer => Engine.GameSerializer;

        private IEngine Engine { get; set; }

        public IActorList Actors => Engine.Actors;

        public IBoard Board => Engine.Board;

        public IList<IPackedBoard> Boards => Engine.Boards;

        public byte[] DumpMemory() => Engine.Memory.Dump();

        public IElementList Elements => Engine.Elements;

        public void ExecuteOnce()
        {
            if (Engine.State.EditorMode)
            {
                // simulate a game cycle for visuals only
                Engine.State.ActIndex = 0;
                Engine.State.GameCycle++;
                if (Engine.State.GameCycle >= MaxGameCycle)
                {
                    Engine.State.GameCycle = 0;
                }

                foreach (var actor in Actors)
                {
                    if (actor.Cycle > 0 && Engine.State.ActIndex%actor.Cycle == Engine.State.GameCycle%actor.Cycle)
                    {
                        Engine.UpdateBoard(actor.Location);
                    }
                    Engine.State.ActIndex++;
                }
            }
        }

        public void PackBoard() => Engine.PackBoard();

        public void Refresh() => Engine.RedrawBoard();

        public byte[] Save()
        {
            using (var mem = new MemoryStream())
            {
                var writer = new BinaryWriter(mem);
                Engine.PackBoard();
                writer.Write((short) WorldData.WorldType);
                writer.Write((short) (Boards.Count - 1));
                writer.Flush();
                GameSerializer.SaveWorld(mem);
                foreach (var board in Boards)
                {
                    GameSerializer.SaveBoardData(mem, board.Data);
                }
                mem.Flush();
                return mem.ToArray();
            }
        }

        public void Save(string filename)
        {
            Engine.Disk.PutFile(filename, Save());
        }

        public void SetBoard(int boardIndex) => Engine.SetBoard(boardIndex);

        public void Start() => Engine.Start();

        public void Stop() => Engine.Stop();

        public ITileGrid Tiles => Engine.Tiles;

        public void UnpackBoard() => Engine.UnpackBoard(Engine.World.BoardIndex);

        public IWorld WorldData => Engine.World;

        public int WorldSize
        {
            get { return GameSerializer.WorldDataCapacity + Boards.Sum(board => board.Data.Length + 2); }
        }

        private void Initialize(ContextEngine engine)
        {
            var resources = new ResourceZipFileSystem(Properties.Resources.resources);
            switch (engine)
            {
                case ContextEngine.Zzt:
                    Engine = new ZztEngine(_config, resources.GetZztMemoryData(), resources.GetZztElementData());
                    break;
                case ContextEngine.SuperZzt:
                    Engine = new SuperZztEngine(_config, resources.GetSuperZztMemoryData(),
                        resources.GetSuperZztElementData());
                    break;
                default:
                    throw Exceptions.InvalidFormat;
            }

            Engine.ClearWorld();
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

        private void LoadAfterType(Stream stream)
        {
            var reader = new BinaryReader(stream);
            int boardCount = reader.ReadInt16();
            GameSerializer.LoadWorld(stream);
            Boards.Clear();
            for (var i = 0; i <= boardCount; i++)
            {
                Boards.Add(new PackedBoard(GameSerializer.LoadBoardData(stream)));
            }
            GameSerializer.UnpackBoard(Engine.Tiles, Engine.Boards[Engine.World.BoardIndex].Data);
            Engine.State.WorldLoaded = true;
        }
    }
}