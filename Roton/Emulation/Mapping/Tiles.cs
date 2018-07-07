using Roton.Core;
using Roton.Core.Collections;

namespace Roton.Emulation.Mapping
{
    public abstract class Tiles : FixedList<ITile>, ITiles
    {
        private readonly IElements _elements;

        protected Tiles(IMemory memory, IElements elements, int offset, int width, int height)
        {
            _elements = elements;
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

        public override string ToString()
        {
            return $"TileGrid ({Width}x{Height})";
        }
        
        public IElement ElementAt(IXyPair location)
        {
            return _elements[this[location].Id];
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
            var element = _elements[tile.Id];

            if (element.Color < 0xF0) return element.Color & 7;
            if (element.Color == 0xFE) return ((tile.Color >> 4) & 0x0F) + 8;
            return tile.Color & 0x0F;
        }        
    }
}