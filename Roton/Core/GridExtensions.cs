using Roton.Extensions;

namespace Roton.Core
{
    public static class GridExtensions
    {
        public static ITile TileAt(this IGrid grid, IXyPair location) =>
            grid[location];

        public static int Adjacent(this IGrid grid, IXyPair location, int id)
        {
            return (location.Y <= 1 || grid[location.Sum(Vector.North)].Id == id ? 1 : 0) |
                   (location.Y >= grid.Height || grid[location.Sum(Vector.South)].Id == id ? 2 : 0) |
                   (location.X <= 1 || grid[location.Sum(Vector.West)].Id == id ? 4 : 0) |
                   (location.X >= grid.Width || grid[location.Sum(Vector.East)].Id == id ? 8 : 0);
        }
    }
}