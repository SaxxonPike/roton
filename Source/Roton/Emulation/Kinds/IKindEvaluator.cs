using Roton.Emulation.Data;

namespace Roton.Emulation.Kinds;

public interface IKindEvaluator
{
    /// <summary>
    /// Reads an optional color and a mandatory element name from the script.
    /// </summary>
    /// <param name="oopContext">
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
    bool TryEval(ref OopContext oopContext, ref Word instruction, out Tile result);

}