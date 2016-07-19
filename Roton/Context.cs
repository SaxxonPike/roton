using Roton.Emulation;
using System.Collections.Generic;

namespace Roton
{
    public sealed partial class Context
    {
        internal MemoryActorCollectionBase ActorMemory { get; private set; }

        public int ActorCapacity => Core.Actors.Capacity;

        public IList<Actor> Actors => Core.Actors;

        public int Board
        {
            get { return Core.Board; }
            set { Core.SetBoard(value); }
        }

        public Board BoardData => Core.BoardData;

        public IList<PackedBoard> Boards => Core.Boards;

        public ContextEngine ContextEngine { get; private set; }

        internal CoreBase Core { get; private set; }

        public Actor CreateActor()
        {
            var result = new Actor();
            result.Heap = Core.Heap;
            return result;
        }

        public IFileSystem Disk
        {
            get { return Core.Disk; }
            set { Core.Disk = value; }
        }

        public IDisplayInfo DisplayInfo => (IDisplayInfo) Core;

        public IList<Element> Elements => Core.Elements;

        public IKeyboard Keyboard
        {
            get { return Core.Keyboard; }
            set { Core.Keyboard = value; }
        }

        public byte[] Memory => Core.Memory.Dump();

        public bool Quiet => Core.Quiet;

        public int ScreenHeight { get; private set; }

        public bool ScreenWide { get; private set; }

        public int ScreenWidth { get; private set; }

        internal SerializerBase Serializer => Core.Serializer;

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

        public IList<Tile> Tiles => Core.Tiles;

        public World WorldData => Core.WorldData;

        public int WorldSize
        {
            get
            {
                var total = Serializer.WorldDataCapacity;
                foreach (var board in Boards)
                {
                    total += board.Data.Length + 2;
                }
                return total;
            }
        }

        internal int WorldType => WorldData.WorldType;
    }
}