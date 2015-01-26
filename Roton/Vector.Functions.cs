using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this.X = x;
            this.Y = y;
        }

        public Vector Clone() 
        {
            return new Vector(this.X, this.Y);
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

        virtual protected void Initialize()
        {
        }

        public bool IsZero
        {
            get { return this.X == 0 && this.Y == 0; }
        }

        public Vector Opposite()
        {
            return new Vector(-this.X, -this.Y);
        }

        public void SetOpposite()
        {
            this.X = -this.X;
            this.Y = -this.Y;
        }

        public void SetTo(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return "[" + this.X.ToString() + ", " + this.Y.ToString() + "]";
        }
    }
}
