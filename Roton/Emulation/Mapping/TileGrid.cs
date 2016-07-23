using Roton.Core;
using Roton.Core.Collections;

namespace Roton.Emulation.Mapping
{
    internal abstract class TileGrid : FixedList<ITile>, ITileGrid
    {
        protected TileGrid(IMemory memory, int offset, int width, int height)
        {
            Memory = memory;
            Offset = offset;
            Height = height;
            Width = width;
        }

        public override int Count => TotalWidth*TotalHeight;

        private IMemory Memory { get; }

        private int Offset { get; }

        private int TotalHeight => Height + 2;

        private int TotalWidth => Width + 2;

        public int Height { get; }

        public ITile this[IXyPair location] => this[location.X*TotalHeight + location.Y];

        public int Width { get; }

        protected override ITile GetItem(int index)
        {
            return new MemoryTile(Memory, Offset + index*2);
        }

        protected override void SetItem(int index, ITile value)
        {
            throw Exceptions.InvalidSet;
        }
    }
}