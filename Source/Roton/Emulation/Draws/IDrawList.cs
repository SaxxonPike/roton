namespace Roton.Emulation.Draws;

/// <summary>
/// Resolves character renderers for element IDs.
/// </summary>
public interface IDrawList
{
    /// <summary>
    /// Gets the character renderer for the specified element ID.
    /// </summary>
    /// <param name="elementId">
    /// Element ID corresponding to the renderer.
    /// </param>
    /// <returns>
    /// The character renderer for the specified element ID, or null if none exists.
    /// </returns>
    IDraw? Get(int elementId);
}