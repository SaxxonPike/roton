namespace Roton.Core
{
    public class Location : IXyPair
    {
        public Location()
        {
        }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public IXyPair Clone()
        {
            return new Location(X, Y);
        }

        public virtual int X { get; set; }
        public virtual int Y { get; set; }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}