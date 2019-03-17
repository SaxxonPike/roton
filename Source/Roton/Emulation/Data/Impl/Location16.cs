namespace Roton.Emulation.Data.Impl
{
    /// <summary>
    ///     A signed 16-bit (X,Y) pair.
    /// </summary>
    public class Location16 : IXyPair
    {
        private int _x;
        private int _y;

        public Location16()
        {
        }

        public Location16(IXyPair source)
        {
            X = source.X;
            Y = source.Y;
        }

        public Location16(int x, int y)
        {
            X = x;
            Y = y;
        }

        public IXyPair Clone()
        {
            return new Location16(X, Y);
        }

        public int X
        {
            get => _x;
            set => _x = (value << 16) >> 16;
        }

        public int Y
        {
            get => _y;
            set => _y = (value << 16) >> 16;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}