namespace Roton.Emulation.Data.Impl
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

        public ITile Clone()
        {
            return new Tile(Id, Color);
        }

        public int Color { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return $"Id: {Id:x2}, Color: {Color:x2}";
        }
    }
}