using System;

namespace Roton.Emulation.Targets;

/// <summary>
/// Resolves label target resolvers by name.
/// </summary>
public interface ITargetList
{
    /// <summary>
    /// Gets the label target resolver with the specified name.
    /// </summary>
    /// <param name="name">
    /// Name of the label target.
    /// </param>
    /// <returns>
    /// Label target resolver with the specified name, or null if no label target resolver is defined.
    /// </returns>
    ITarget? Get(ReadOnlySpan<char> name);
}