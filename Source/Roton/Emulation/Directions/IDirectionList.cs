using System;

namespace Roton.Emulation.Directions;

/// <summary>
/// Resolves vector evaluators by name in the scripting engine.
/// </summary>
public interface IDirectionList
{
    /// <summary>
    /// Gets the vector evaluator for the specified name.
    /// </summary>
    /// <param name="name">
    /// Name of the vector evaluator.
    /// </param>
    /// <returns>
    /// The vector evaluator that corresponds to the specified name or null if no evaluator is found.
    /// </returns>
    IDirection? Get(ReadOnlySpan<char> name);
}