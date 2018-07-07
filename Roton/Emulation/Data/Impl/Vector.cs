namespace Roton.Core
{
    /// <summary>
    ///     A signed 16-bit (X,Y) pair.
    /// </summary>
    public class Vector : IXyPair
    {
        private int _x;
        private int _y;

        public Vector()
        {
        }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static IXyPair East => new Vector(1, 0);
        public static IXyPair Idle => new Vector(0, 0);
        public static IXyPair North => new Vector(0, -1);
        public static IXyPair South => new Vector(0, 1);
        public static IXyPair West => new Vector(-1, 0);

        public IXyPair Clone()
        {
            return new Vector(X, Y);
        }

        public int X
        {
            get { return _x; }
            set { _x = (value << 16) >> 16; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = (value << 16) >> 16; }
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}