namespace Roton.Core
{
    public static class GridExtensions
    {
        public static ITile TileAt(this IGrid grid, IXyPair location) =>
            grid[location];
    }
}