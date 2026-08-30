using Roton.Emulation.Data;

namespace Roton.Emulation.Commands;

/// <summary>
/// Represents a command that can be processed by the scripting engine.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes a script command.
    /// </summary>
    /// <param name="context">
    /// Script context.
    /// </param>
    /// <param name="instruction">
    /// Current offset within the script, which may be modified by the command.
    /// </param>
    void Execute(ref OopContext context, ref Word instruction);
}