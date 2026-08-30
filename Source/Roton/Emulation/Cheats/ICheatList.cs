using System;

namespace Roton.Emulation.Cheats;

/// <summary>
/// Resolves cheat codes that correspond to entered text.
/// </summary>
public interface ICheatList
{
    /// <summary>
    /// Gets the cheat code for the specified text.
    /// </summary>
    /// <param name="name">
    /// Entered text.
    /// </param>
    /// <returns>
    /// The cheat code, or null if no matching cheat code was found.
    /// </returns>
    ICheat? Get(ReadOnlySpan<char> name);
}