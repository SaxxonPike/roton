using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ITileEvaluator
{
    /// <summary>
    /// Reads an optional color and a mandatory element name from the script.
    /// </summary>
    /// <param name="context">
    /// Execution context.
    /// </param>
    /// <param name="instruction">
    /// Instruction pointer.
    /// </param>
    /// <param name="result">
    /// A tile that contains the element and color that was read. If no valid Kind was read,
    /// null is returned.
    /// </param>
    /// <returns>
    /// True if the kind was successfully parsed, false otherwise.
    /// </returns>
    bool TryEval(ref OopContext context, ref Word instruction, out Tile result);

}