namespace Roton.Core
{
    public class Location : IXyPair
    {
        public virtual int X { get; set; }
        public virtual int Y { get; set; }

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

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}