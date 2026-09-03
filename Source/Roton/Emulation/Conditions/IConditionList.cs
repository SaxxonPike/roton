using System;

namespace Roton.Emulation.Conditions;

/// <summary>
/// Resolves boolean conditions by name.
/// </summary>
public interface IConditionList
{
    /// <summary>
    /// Gets the boolean condition for the specified name.
    /// </summary>
    /// <param name="name">
    /// Name of the boolean condition.
    /// </param>
    /// <returns>
    /// The boolean condition for the specified name, or null if no matching condition exists.
    /// </returns>
    ICondition? Get(ReadOnlySpan<char> name);
}