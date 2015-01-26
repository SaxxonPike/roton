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
        internal MemoryActorCollectionBase ActorMemory
        {
            get;
            private set;
        }

        public int ActorCapacity
        {
            get { return Core.Actors.Capacity; }
        }

        public IList<Actor> Actors
        {
            get { return Core.Actors; }
        }

        public int Board
        {
            get
            {
                return Core.Board;
            }
            set
            {
                Core.SetBoard(value);
            }
        }

        public Board BoardData
        {
            get { return Core.BoardData; }
        }

        public IList<PackedBoard> Boards
        {
            get { return Core.Boards; }
        }

        public ContextEngine ContextEngine
        {
            get;
            private set;
        }

        internal CoreBase Core
        {
            get;
            private set;
        }

        public Actor CreateActor()
        {
            Actor result = new Actor();
            result.Heap = Core.Heap;
            return result;
        }

        internal SerializerBase Disk
        {
            get { return Core.Disk; }
        }

        public IDisplayInfo DisplayInfo
        {
            get { return (IDisplayInfo)Core; }
        }

        public IList<Element> Elements
        {
            get { return Core.Elements; }
        }

        public IKeyboard Keyboard
        {
            get { return Core.Keyboard; }
            set { Core.Keyboard = value; }
        }

        public byte[] Memory
        {
            get
            {
                return Core.Memory.Dump();
            }
        }

        public bool Quiet
        {
            get { return Core.Quiet; }
        }

        public int ScreenHeight
        {
            get;
            private set;
        }

        public bool ScreenWide
        {
            get;
            private set;
        }

        public int ScreenWidth
        {
            get;
            private set;
        }

        public ISpeaker Speaker
        {
            get { return Core.Speaker; }
            set { Core.Speaker = value; }
        }

        public void Start()
        {
            Core.Start();
        }

        public void Stop()
        {
            Core.Stop();
        }

        public ITerminal Terminal
        {
            get { return Core.Terminal; }
            set
            {
                Core.Terminal = value;
                Core.Terminal.SetSize(ScreenWidth, ScreenHeight, ScreenWide);
                Core.Terminal.Write(0, 0, "Terminal initialized.", 0x0F);
            }
        }

        public Tile TileAt(Location l)
        {
            return Core.Tiles[l];
        }

        public Tile TileAt(int x, int y)
        {
            return Core.Tiles[new Location(x, y)];
        }

        public IList<Tile> Tiles
        {
            get { return Core.Tiles; }
        }

        public World WorldData
        {
            get { return Core.WorldData; }
        }

        public int WorldSize
        {
            get
            {
                int total = Disk.WorldDataCapacity;
                foreach (var board in Boards)
                {
                    total += board.Data.Length + 2;
                }
                return total;
            }
        }

        internal int WorldType
        {
            get { return WorldData.WorldType; }
        }
    }
}
