namespace Roton
{
    public partial class Tile
    {
        public Tile()
        {
        }

        public Tile(int id, int color)
        {
            Id = id;
            Color = color;
        }

        public Tile Clone()
        {
            return new Tile(Id, Color);
        }

        public void CopyFrom(Tile tile)
        {
            Id = tile.Id;
            Color = tile.Color;
        }

        public bool Matches(Tile other)
        {
            return Id == other.Id && Color == other.Color;
        }

        public bool Matches(int id, int color)
        {
            return Id == id && Color == color;
        }

        public void SetTo(int id, int color)
        {
            Id = id;
            Color = color;
        }

        public void SetTo(Element element)
        {
            Id = element.Index;
        }

        public void SetTo(Element element, int color)
        {
            SetTo(element);
            Color = color;
        }

        public override string ToString()
        {
            return "Id: " + Id.ToHex8() + ", Color: " + Color.ToHex8();
        }
    }
}