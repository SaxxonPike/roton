using Roton.Emulation.Data;

namespace Roton.Emulation.Directions;

/// <summary>
/// Represents a vector within the scripting engine.
/// </summary>
public interface IDirection
{
    /// <summary>
    /// Evaluates the vector for the given script context.
    /// </summary>
    /// <param name="context">
    /// Script context.
    /// </param>
    /// <param name="instruction">
    /// Current offset within the script, which may be modified by the command.
    /// </param>
    /// <returns>
    /// The evaluated vector. If not found or invalid, <see cref="Vector.Idle"/> is returned.
    /// </returns>
    Vector Execute(ref OopContext context, ref Word instruction);
}