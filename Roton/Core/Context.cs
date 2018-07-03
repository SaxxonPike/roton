using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Roton.Emulation.Mapping;
using Roton.Emulation.SuperZZT;
using Roton.Emulation.ZZT;
using Roton.Events;
using Roton.Resources;

namespace Roton.Core
{
    public sealed class Context : IContext
    {
        public event EventHandler Terminated;

        private const int MaxGameCycle = 420;

        public Context(
            IActorList actorList, 
            IBoard board, 
            IDrumBank drumBank, 
            IElementList elementList, 
            ITileGrid tileGrid, 
            IWorld world)
        {
            
        }

        private IGameSerializer GameSerializer => Engine.GameSerializer;

        private IEngine Engine { get; set; }

        public IActorList Actors => Engine.Actors;

        public IBoard Board => Engine.Board;

        public IList<IPackedBoard> Boards => Engine.Boards;

        public IDrumBank Drums => Engine.DrumBank;

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
            var resources = ResourceZipFileSystem.System;
            switch (engine)
            {
                case ContextEngine.Zzt:
                    Engine = new ZztEngine(_config, resources.GetFile("memory-zzt.bin"), resources.GetFile("elements-zzt.bin"));
                    break;
                case ContextEngine.SuperZzt:
                    Engine = new SuperZztEngine(_config, resources.GetFile("memory-szzt.bin"), resources.GetFile("elements-szzt.bin"));
                    break;
                default:
                    throw Exceptions.InvalidFormat;
            }

            Engine.RequestReplaceContext += OnEngineRequestReplaceContext;
            Engine.Terminated += (s, e) => Terminated?.Invoke(s, e);
            Engine.ClearWorld();
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