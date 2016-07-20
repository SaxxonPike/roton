using Roton.Core;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryTileCollectionBase : FixedList<ITile>, ITileGrid
    {
        public MemoryTileCollectionBase(IMemory memory, int offset, int width, int height)
        {
            Memory = memory;
            Offset = offset;
            Height = height;
            Width = width;
        }

        protected override ITile GetItem(int index)
        {
            return new MemoryTile(Memory, Offset + index*2);
        }

        protected override void SetItem(int index, ITile value)
        {
            throw Exceptions.InvalidSet;
        }

        public ITile this[IXyPair location] => this[location.X*TotalHeight + location.Y];

        public override int Count => TotalWidth*TotalHeight;

        public int Height { get; }

        public IMemory Memory { get; }

        public int Offset { get; }

        private int TotalHeight => Height + 2;

        private int TotalWidth => Width + 2;

        public int Width { get; }
    }
}