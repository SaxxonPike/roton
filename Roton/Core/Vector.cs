namespace Roton.Core
{
    public class Vector : IXyPair
    {
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

        public virtual int X { get; set; }
        public virtual int Y { get; set; }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}