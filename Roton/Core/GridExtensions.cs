using Roton.Extensions;

namespace Roton.Core
{
    public static class GridExtensions
    {
        public static int Adjacent(this ITiles tiles, IXyPair location, int id)
        {
            return (location.Y <= 1 || tiles[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= tiles.Height || tiles[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || tiles[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= tiles.Width || tiles[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }
    }
}