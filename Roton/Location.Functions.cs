namespace Roton
{
    public partial class Location
    {
        internal Location()
        {
            Initialize();
        }

        internal Location(int x, int y)
        {
            X = x;
            Y = y;
            Initialize();
        }

        public void Add(Location location)
        {
            X += location.X;
            Y += location.Y;
        }

        public void Add(Vector vector)
        {
            X += vector.X;
            Y += vector.Y;
        }

        public void Add(int x, int y)
        {
            X += x;
            Y += y;
        }

        public Location Clone()
        {
            return new Location(X, Y);
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

        public Location Difference(Location other)
        {
            return new Location(X - other.X, Y - other.Y);
        }

        public Location Difference(Vector other)
        {
            return new Location(X - other.X, Y - other.Y);
        }

        public Location Difference(int x, int y)
        {
            return new Location(X - x, Y - y);
        }

        protected virtual void Initialize()
        {
        }

        public bool Matches(Location other)
        {
            return X == other.X && Y == other.Y;
        }

        public bool Matches(int x, int y)
        {
            return X == x && Y == y;
        }

        public void SetTo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Subtract(Location location)
        {
            X -= location.X;
            Y -= location.Y;
        }

        public void Subtract(Vector vector)
        {
            X -= vector.X;
            Y -= vector.Y;
        }

        public void Subtract(int x, int y)
        {
            X -= x;
            Y -= y;
        }

        public Location Sum(Location other)
        {
            return new Location(X + other.X, Y + other.Y);
        }

        public Location Sum(Vector other)
        {
            return new Location(X + other.X, Y + other.Y);
        }

        public Location Sum(int x, int y)
        {
            return new Location(X + x, Y + y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}