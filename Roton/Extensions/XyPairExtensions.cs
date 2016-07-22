using Roton.Core;

namespace Roton.Extensions
{
    public static class XyPairExtensions
    {
        public static void Add(this IXyPair pair, IXyPair other)
        {
            pair.X += other.X;
            pair.Y += other.Y;
        }

        public static void Add(this IXyPair pair, int x, int y)
        {
            pair.X += x;
            pair.Y += y;
        }

        public static void CopyFrom(this IXyPair pair, IXyPair other)
        {
            pair.X = other.X;
            pair.Y = other.Y;
        }

        public static IXyPair Difference(this IXyPair pair, IXyPair other)
        {
            var clone = pair.Clone();
            clone.Subtract(other);
            return clone;
        }

        public static IXyPair Difference(this IXyPair pair, int x, int y)
        {
            var clone = pair.Clone();
            clone.Subtract(x, y);
            return clone;
        }

        public static bool Matches(this IXyPair pair, IXyPair other)
        {
            return pair.X == other.X && pair.Y == other.Y;
        }

        public static bool Matches(this IXyPair pair, int x, int y)
        {
            return pair.X == x && pair.Y == y;
        }

        public static void SetTo(this IXyPair pair, int x, int y)
        {
            pair.X = x;
            pair.Y = y;
        }

        public static void Subtract(this IXyPair pair, IXyPair location)
        {
            pair.X -= location.X;
            pair.Y -= location.Y;
        }

        public static void Subtract(this IXyPair pair, int x, int y)
        {
            pair.X -= x;
            pair.Y -= y;
        }

        public static IXyPair Sum(this IXyPair pair, IXyPair other)
        {
            var clone = pair.Clone();
            clone.Add(other);
            return clone;
        }

        public static IXyPair Sum(this IXyPair pair, int x, int y)
        {
            var clone = pair.Clone();
            clone.Add(x, y);
            return clone;
        }

        public static IXyPair Clockwise(this IXyPair pair)
        {
            var clone = pair.Clone();
            clone.SetClockwise();
            return clone;
        }

        public static IXyPair CounterClockwise(this IXyPair pair)
        {
            var clone = pair.Clone();
            clone.SetCounterClockwise();
            return clone;
        }

        public static bool IsNonZero(this IXyPair pair)
        {
            return pair.X != 0 || pair.Y != 0;
        }

        public static bool IsZero(this IXyPair pair)
        {
            return pair.X == 0 && pair.Y == 0;
        }

        public static IXyPair Product(this IXyPair pair, int value)
        {
            var clone = pair.Clone();
            clone.SetTo(clone.X*value, clone.Y*value);
            return clone;
        }

        public static IXyPair Opposite(this IXyPair pair)
        {
            var clone = pair.Clone();
            clone.SetOpposite();
            return clone;
        }

        public static void SetClockwise(this IXyPair pair)
        {
            pair.SetTo(-pair.Y, pair.X);
        }

        public static void SetCounterClockwise(this IXyPair pair)
        {
            pair.SetTo(pair.Y, -pair.X);
        }

        public static void SetOpposite(this IXyPair pair)
        {
            pair.SetTo(-pair.X, -pair.Y);
        }

        public static IXyPair Swap(this IXyPair pair)
        {
            var clone = pair.Clone();
            clone.SetTo(pair.Y, pair.X);
            return clone;
        }
    }
}