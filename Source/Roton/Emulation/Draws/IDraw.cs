using Roton.Emulation.Data;

namespace Roton.Emulation.Draws;

/// <summary>
/// Represents a character renderer.
/// </summary>
public interface IDraw
{
    /// <summary>
    /// Renders the character at the specified location.
    /// </summary>
    /// <param name="location">
    /// Location to render.
    /// </param>
    /// <returns>
    /// The character that was rendered corresponding to the location.
    /// </returns>
    AnsiChar Draw(Location location);
}