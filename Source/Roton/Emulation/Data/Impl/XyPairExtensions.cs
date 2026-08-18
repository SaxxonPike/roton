// using Roton.Emulation.Infrastructure;
//
// namespace Roton.Emulation.Data.Impl;
//
// public static class XyPairExtensions
// {
//     extension(IXyPair pair)
//     {
//         public void Add(IXyPair other)
//         {
//             pair.X += other.X;
//             pair.Y += other.Y;
//         }
//
//         public void Add(int x, int y)
//         {
//             pair.X += x;
//             pair.Y += y;
//         }
//
//         public IXyPair Clockwise()
//         {
//             var clone = pair.Clone();
//             clone.SetClockwise();
//             return clone;
//         }
//
//         public void CopyFrom(IXyPair other)
//         {
//             pair.X = other.X;
//             pair.Y = other.Y;
//         }
//
//         public IXyPair CounterClockwise()
//         {
//             var clone = pair.Clone();
//             clone.SetCounterClockwise();
//             return clone;
//         }
//
//         public IXyPair Difference(IXyPair other)
//         {
//             var clone = pair.Clone();
//             clone.Subtract(other);
//             return clone;
//         }
//
//         public IXyPair Difference(int x, int y)
//         {
//             var clone = pair.Clone();
//             clone.Subtract(x, y);
//             return clone;
//         }
//
//         public int DistanceTo(IXyPair b)
//         {
//             return (pair.Y - b.Y).Square() * 2 + (pair.X - b.X).Square();
//         }
//
//         public bool IsNonZero()
//         {
//             return pair.X != 0 || pair.Y != 0;
//         }
//
//         public bool IsZero()
//         {
//             return pair.X == 0 && pair.Y == 0;
//         }
//
//         public bool Matches(IXyPair other)
//         {
//             return pair.X == other.X && pair.Y == other.Y;
//         }
//
//         public bool Matches(int x, int y)
//         {
//             return pair.X == x && pair.Y == y;
//         }
//
//         public IXyPair Opposite()
//         {
//             var clone = pair.Clone();
//             clone.SetOpposite();
//             return clone;
//         }
//
//         public IXyPair Product(int value)
//         {
//             var clone = pair.Clone();
//             clone.SetTo(clone.X * value, clone.Y * value);
//             return clone;
//         }
//
//         public void SetClockwise()
//         {
//             pair.SetTo(-pair.Y, pair.X);
//         }
//
//         public void SetCounterClockwise()
//         {
//             pair.SetTo(pair.Y, -pair.X);
//         }
//
//         public void SetOpposite()
//         {
//             pair.SetTo(-pair.X, -pair.Y);
//         }
//
//         public void SetTo(int x, int y)
//         {
//             pair.X = x;
//             pair.Y = y;
//         }
//
//         public void Subtract(Location location)
//         {
//             pair.X -= location.X;
//             pair.Y -= location.Y;
//         }
//
//         public void Subtract(int x, int y)
//         {
//             pair.X -= x;
//             pair.Y -= y;
//         }
//
//         public IXyPair Sum(IXyPair other)
//         {
//             var clone = pair.Clone();
//             clone.Add(other);
//             return clone;
//         }
//
//         public IXyPair Sum(int x, int y)
//         {
//             var clone = pair.Clone();
//             clone.Add(x, y);
//             return clone;
//         }
//
//         public IXyPair Swap()
//         {
//             var clone = pair.Clone();
//             clone.SetTo(pair.Y, pair.X);
//             return clone;
//         }
//     }
// }