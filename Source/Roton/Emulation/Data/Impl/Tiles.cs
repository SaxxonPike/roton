using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl
{
    public abstract class Tiles : FixedList<ITile>, ITiles
    {
        private readonly IElementList _elementList;
        private readonly IMemory _memory;
        private readonly int _offset;

        protected Tiles(IMemory memory, IElementList elementList, int offset, int width, int height)
        {
            _elementList = elementList;
            _memory = memory;
            _offset = offset;
            Height = height;
            Width = width;
        }

        public override int Count => TotalWidth * TotalHeight;

        private int TotalHeight => Height + 2;

        private int TotalWidth => Width + 2;

        public int Height { get; }

        public ITile this[IXyPair location] => this[location.X * TotalHeight + location.Y];

        public int Width { get; }

        protected override ITile GetItem(int index)
        {
            return new MemoryTile(_memory, _offset + index * 2);
        }

        protected override void SetItem(int index, ITile value)
        {
            throw Exceptions.InvalidSet;
        }

        public override string ToString()
        {
            return $"TileGrid ({Width}x{Height})";
        }

        public IElement ElementAt(IXyPair location)
        {
            return _elementList[this[location].Id];
        }

        public bool FindTile(ITile kind, IXyPair location)
        {
            location.X++;
            while (location.Y <= Height)
            {
                while (location.X <= Width)
                {
                    var tile = this[location];
                    if (tile.Id == kind.Id)
                    {
                        if (kind.Color == 0 || ColorMatch(this[location]) == kind.Color)
                        {
                            return true;
                        }
                    }

                    location.X++;
                }

                location.X = 1;
                location.Y++;
            }

            return false;
        }

        private int ColorMatch(ITile tile)
        {
            var element = _elementList[tile.Id];

            if (element.Color < 0xF0) return element.Color & 7;
            if (element.Color == 0xFE) return ((tile.Color >> 4) & 0x0F) + 8;
            return tile.Color & 0x0F;
        }
    }
}