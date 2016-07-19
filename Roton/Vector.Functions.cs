namespace Roton
{
    public partial class Vector
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

        public Vector Clockwise => new Vector(-Y, X);

        public Vector Clone()
        {
            return new Vector(X, Y);
        }

        public void CopyFrom(Location location)
        {
            X = location.X;
            Y = location.Y;
        }

        public void CopyFrom(Vector vector)
        {
            X = vector.X;
            Y = vector.Y;
        }

        public Vector CounterClockwise => new Vector(Y, -X);

        protected virtual void Initialize()
        {
        }

        public bool IsNonZero => !IsZero;

        public bool IsZero => X == 0 && Y == 0;

        public Vector Multiply(int value)
        {
            return new Vector(X*value, Y*value);
        }

        public Vector Opposite => new Vector(-X, -Y);

        public void SetClockwise()
        {
            CopyFrom(Clockwise);
        }

        public void SetCounterClockwise()
        {
            CopyFrom(CounterClockwise);
        }

        public void SetOpposite()
        {
            CopyFrom(Opposite);
        }

        public void SetTo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector Swap => new Vector(Y, X);

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}