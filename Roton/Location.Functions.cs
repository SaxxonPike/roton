using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this.X = x;
            this.Y = y;
            Initialize();
        }

        public void Add(Location location)
        {
            this.X += location.X;
            this.Y += location.Y;
        }

        public void Add(Vector vector)
        {
            this.X += vector.X;
            this.Y += vector.Y;
        }

        public void Add(int x, int y)
        {
            this.X += x;
            this.Y += y;
        }

        public Location Clone()
        {
            return new Location(this.X, this.Y);
        }

        public void CopyFrom(Location location)
        {
            this.X = location.X;
            this.Y = location.Y;
        }

        public void CopyFrom(Vector vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }

        public Location Difference(Location other)
        {
            return new Location(this.X - other.X, this.Y - other.Y);
        }

        public Location Difference(Vector other)
        {
            return new Location(this.X - other.X, this.Y - other.Y);
        }

        public Location Difference(int x, int y)
        {
            return new Location(this.X - x, this.Y - y);
        }

        virtual protected void Initialize()
        {
        }

        public bool Matches(Location other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public bool Matches(int x, int y)
        {
            return this.X == x && this.Y == y;
        }

        public void SetTo(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Subtract(Location location)
        {
            this.X -= location.X;
            this.Y -= location.Y;
        }

        public void Subtract(Vector vector)
        {
            this.X -= vector.X;
            this.Y -= vector.Y;
        }

        public void Subtract(int x, int y)
        {
            this.X -= x;
            this.Y -= y;
        }

        public Location Sum(Location other)
        {
            return new Location(this.X + other.X, this.Y + other.Y);
        }

        public Location Sum(Vector other)
        {
            return new Location(this.X + other.X, this.Y + other.Y);
        }

        public Location Sum(int x, int y)
        {
            return new Location(this.X + x, this.Y + y);
        }

        public override string ToString()
        {
            return "(" + this.X.ToString() + ", " + this.Y.ToString() + ")";
        }
    }
}
