using System;

namespace Roton
{
    public class Vector : IXyPair
    {
        public Vector()
        {
            Initialize();
        }

        public Vector(int x, int y)
        {
            Initialize();
            X = x;
            Y = y;
        }

        public virtual int X { get; set; }
        public virtual int Y { get; set; }

        public static IXyPair East => new Vector(1, 0);
        public static IXyPair North => new Vector(0, -1);
        public static IXyPair South => new Vector(0, 1);
        public static IXyPair West => new Vector(-1, 0);

        public IXyPair Clone()
        {
            return new Vector(X, Y);
        }

        public void CopyFrom(IXyPair other)
        {
            X = other.X;
            Y = other.Y;
        }

        protected virtual void Initialize()
        {
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}