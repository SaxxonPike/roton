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

        public Vector Clockwise
        {
            get
            {
                return new Vector(-this.Y, this.X);
            }
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

        public Vector CounterClockwise
        {
            get
            {
                return new Vector(this.Y, -this.X);
            }
        }

        virtual protected void Initialize()
        {
        }

        public bool IsNonZero
        {
            get { return !IsZero; }
        }

        public bool IsZero
        {
            get { return this.X == 0 && this.Y == 0; }
        }

        public Vector Multiply(int value)
        {
            return new Vector(this.X * value, this.Y * value);
        }

        public Vector Opposite
        {
            get
            {
                return new Vector(-this.X, -this.Y);
            }
        }

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
            this.X = x;
            this.Y = y;
        }

        public Vector Swap
        {
            get
            {
                return new Vector(this.Y, this.X);
            }
        }

        public override string ToString()
        {
            return "[" + this.X.ToString() + ", " + this.Y.ToString() + "]";
        }
    }
}
