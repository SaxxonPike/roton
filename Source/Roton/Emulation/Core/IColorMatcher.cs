namespace Roton.Emulation.Core;

public interface IColorMatcher
{
    /// <remarks>
    /// RoZ: GetColorForTileMatch
    /// </remarks>
    int GetColorMatchValue(int color);
}