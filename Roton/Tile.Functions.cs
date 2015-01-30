using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public partial class Tile
    {
        public Tile()
        {
        }

        public Tile(int id, int color)
        {
            this.Id = id;
            this.Color = color;
        }

        public Tile Clone()
        {
            return new Tile(this.Id, this.Color);
        }

        public void CopyFrom(Tile tile)
        {
            this.Id = tile.Id;
            this.Color = tile.Color;
        }

        public bool Matches(Tile other)
        {
            return this.Id == other.Id && this.Color == other.Color;
        }

        public bool Matches(int id, int color)
        {
            return this.Id == id && this.Color == color;
        }

        public void SetTo(int id, int color)
        {
            this.Id = id;
            this.Color = color;
        }

        public void SetTo(Element element)
        {
            this.Id = element.Index;
        }

        public void SetTo(Element element, int color)
        {
            SetTo(element);
            this.Color = color;
        }

        public override string ToString()
        {
            return "Id: " + Id.ToHex8() + ", Color: " + Color.ToHex8();
        }
    }
}
