using Roton.Emulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton
{
    sealed public partial class Context
    {
        public Context(string filename, bool editor)
        {
            using (MemoryStream mem = new MemoryStream(File.ReadAllBytes(filename)))
            {
                Initialize(mem, editor);
            }
        }

        public Context(byte[] data, bool editor)
        {
            using (MemoryStream mem = new MemoryStream(data))
            {
                Initialize(mem, editor);
            }
        }

        public Context(ContextEngine engine, bool editor)
        {
            Initialize(engine, editor);
        }

        public Context(Stream stream, bool editor)
        {
            Initialize(stream, editor);
        }

        public void ExecuteOnce()
        {
            if (Core.EditorMode)
            {
                // simulate a game cycle for visuals only
                Core.ActIndex = 0;
                Core.GameCycle++;
                if (Core.GameCycle >= 420)
                {
                    Core.GameCycle = 0;
                }
                foreach (var actor in Actors)
                {
                    if (actor.Cycle > 0 && Core.ActIndex % actor.Cycle == Core.GameCycle % actor.Cycle)
                    {
                        Core.UpdateBoard(actor.Location);
                    }
                    Core.ActIndex++;
                }
            }
            else
            {
            }
        }

        public int Height
        {
            get { return Core.Height; }
        }

        internal void InitBoard(int board)
        {
        }

        private void Initialize(ContextEngine engine, bool editor)
        {
            ContextEngine = engine;
            switch (engine)
            {
                case ContextEngine.ZZT:
                    Core = new Emulation.ZZT.Core();
                    ScreenWidth = 80;
                    ScreenHeight = 25;
                    ScreenWide = false;
                    break;
                case ContextEngine.SuperZZT:
                    Core = new Emulation.SuperZZT.Core();
                    ScreenWidth = 40;
                    ScreenHeight = 25;
                    ScreenWide = true;
                    break;
                default:
                    throw Exceptions.InvalidFormat;
            }

            if (editor)
            {
                // editor mode will always show the full board
                ScreenWidth = Width;
                ScreenHeight = Height;
            }

            Core.ClearWorld();
            Core.EditorMode = editor;
            Core.Disk = new FileSystem();
        }

        private void Initialize(Stream stream, bool editor)
        {
            ContextEngine engine;
            BinaryReader reader = new BinaryReader(stream);
            switch (reader.ReadInt16())
            {
                case -1:
                    engine = ContextEngine.ZZT;
                    break;
                case -2:
                    engine = ContextEngine.SuperZZT;
                    break;
                default:
                    throw Exceptions.UnknownFormat;
            }
            Initialize(engine, editor);
            LoadAfterType(stream);
        }

        internal void Load(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            if (reader.ReadInt16() != WorldData.WorldType)
            {
                throw Exceptions.InvalidFormat;
            }
            else
            {
                LoadAfterType(stream);
            }
        }

        private void LoadAfterType(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int boardCount = reader.ReadInt16();
            Serializer.LoadWorld(stream);
            Boards.Clear();
            for (int i = 0; i <= boardCount; i++)
            {
                Boards.Add(new PackedBoard(Serializer.LoadBoardData(stream)));
            }
            Serializer.UnpackBoard(Core.Tiles, Core.Boards[Core.Board].Data);
            Core.WorldLoaded = true;
        }

        public void PackBoard()
        {
            Core.PackBoard();
        }

        public void Refresh()
        {
            
        }

        public byte[] Save()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                BinaryWriter writer = new BinaryWriter(mem);
                Core.PackBoard();
                writer.Write((Int16)WorldData.WorldType);
                writer.Write((Int16)(Boards.Count - 1));
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
            File.WriteAllBytes(filename, this.Save());
        }

        public void Save(Stream output)
        {
            byte[] data = this.Save();
            output.Write(data, 0, data.Length);
        }

        public void SetBoard(int boardIndex)
        {
            Core.SetBoard(boardIndex);
        }

        public void UnpackBoard()
        {
            Core.UnpackBoard(Core.Board);
        }

        public int Width
        {
            get { return Core.Width; }
        }
    }
}
