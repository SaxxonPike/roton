using Roton.Emulation;
using System.Collections.Generic;
using System.Linq;

namespace Roton
{
    public sealed partial class Context
    {
        public int ActorCapacity => Core.Actors.Capacity;

        public IList<IActor> Actors => Core.Actors;

        public int Board
        {
            get { return Core.Board; }
            set { Core.SetBoard(value); }
        }

        public Board BoardData => Core.BoardData;

        public IList<PackedBoard> Boards => Core.Boards;

        public ContextEngine ContextEngine { get; private set; }

        private CoreBase Core { get; set; }

        public Actor CreateActor()
        {
            var result = new Actor();
            return result;
        }

        public IList<Element> Elements => Core.Elements;

        public IKeyboard Keyboard
        {
            get { return Core.Keyboard; }
            set { Core.Keyboard = value; }
        }

        public byte[] Memory => Core.Memory.Dump();

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

        public World WorldData => Core.WorldData;

        public int WorldSize
        {
            get
            {
                return Serializer.WorldDataCapacity + Boards.Sum(board => board.Data.Length + 2);
            }
        }
    }
}