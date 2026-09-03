using Roton.Emulation.Data;

namespace Roton.Emulation.Items;

public interface IItemEvaluator
{
    /// <summary>
    /// Reads an item name from the script.
    /// </summary>
    /// <param name="context">
    ///     Execution context.
    /// </param>
    /// <param name="instruction">
    ///     Instruction pointer.
    /// </param>
    /// <param name="result">
    ///     A reference to the item value.
    /// </param>
    /// <returns>
    /// True if the item was successfully parsed, false otherwise.
    /// </returns>
    bool TryEval(ref OopContext context, ref Word instruction, out IItem? result);

}