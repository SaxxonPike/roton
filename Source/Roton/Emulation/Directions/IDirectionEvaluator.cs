using Roton.Emulation.Data;

namespace Roton.Emulation.Directions;

public interface IDirectionEvaluator
{
    /// <summary>
    /// Reads a direction name from the script, then converts it to a vector.
    /// </summary>
    /// <param name="context">
    /// Execution context.
    /// </param>
    /// <param name="instruction">
    /// Instruction pointer.
    /// </param>
    /// <param name="result">
    /// A vector that represents the direction that was parsed.
    /// </param>
    /// <returns>
    /// True if the direction was successfully parsed, false otherwise.
    /// </returns>
    bool TryEval(ref OopContext context, ref Word instruction, out Vector result);
}