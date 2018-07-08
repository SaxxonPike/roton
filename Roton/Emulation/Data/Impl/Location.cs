namespace Roton.Emulation.Data.Impl
{
    /// <summary>
    ///     An unsigned 8-bit (X,Y) pair.
    /// </summary>
    public class Location : IXyPair
    {
        private int _x;
        private int _y;

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

        public int X
        {
            get { return _x; }
            set { _x = value & 0xFF; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value & 0xFF; }
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}