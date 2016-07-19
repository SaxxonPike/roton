namespace Roton.Emulation
{
    internal abstract class MemoryTileCollectionBase : FixedList<Tile>
    {
        public MemoryTileCollectionBase(Memory memory, int offset, int width, int height)
        {
            Memory = memory;
            Offset = offset;
            Height = height;
            Width = width;
        }

        protected override Tile GetItem(int index)
        {
            return new MemoryTile(Memory, Offset + index*2);
        }

        protected override void SetItem(int index, Tile value)
        {
            throw Exceptions.InvalidSet;
        }

        public Tile this[Location location] => this[location.X*TotalHeight + location.Y];

        public override int Count => TotalWidth*TotalHeight;

        public int Height { get; }

        public Memory Memory { get; }

        public int Offset { get; }

        private int TotalHeight => Height + 2;

        private int TotalWidth => Width + 2;

        public int Width { get; }
    }
}