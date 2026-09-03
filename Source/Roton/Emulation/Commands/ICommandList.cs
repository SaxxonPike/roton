using System;

namespace Roton.Emulation.Commands;

/// <summary>
/// Resolves command names to executable script commands.
/// </summary>
public interface ICommandList
{
    /// <summary>
    /// Gets an executable script for the specified name.
    /// </summary>
    /// <param name="name">
    /// Name of the script command.
    /// </param>
    /// <returns>
    /// The script command that corresponds to the specified name or null if no such command exists.
    /// </returns>
    ICommand? Get(ReadOnlySpan<char> name);
}