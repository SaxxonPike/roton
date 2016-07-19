namespace Roton
{
    public class Tile : ITile
    {
        public Tile()
        {
        }

        public Tile(int id, int color)
        {
            Id = id;
            Color = color;
        }

        public virtual int Color { get; set; }
        public virtual int Id { get; set; }

        public ITile Clone()
        {
            return new Tile(Id, Color);
        }

        public override string ToString()
        {
            return $"Id: {Id:x2}, Color: {Color:x2}";
        }
    }
}