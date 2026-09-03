using System;

namespace Roton.Emulation.Colors;

/// <summary>
/// Resolves names and IDs to script colors.
/// </summary>
public interface IColorList
{
    /// <summary>
    /// Gets the color with the specified name.
    /// </summary>
    /// <param name="name">
    /// Name of the color.
    /// </param>
    /// <returns>
    /// A color corresponding to the specified name, or null if no matching color was found.
    /// </returns>
    IColor? Get(ReadOnlySpan<char> name);
    
    /// <summary>
    /// Gets the color with the specified ID.
    /// </summary>
    /// <param name="id">
    /// ID of the color.
    /// </param>
    /// <returns>
    /// A color corresponding to the specified ID, or null if no matching color was found.
    /// </returns>
    /// <remarks>
    /// The ID maps to indices in a color array - the first color with ID 0 should be blue.
    /// </remarks>
    IColor? Get(int id);
}