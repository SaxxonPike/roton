using Roton.Emulation.Data;

namespace Roton.Emulation.Conditions;

public interface IConditionEvaluator
{
    /// <summary>
    /// Reads a condition name from the script, then evaluates it.
    /// </summary>
    /// <param name="oopContext">
    /// Execution context.
    /// </param>
    /// <param name="instruction">
    /// Instruction pointer.
    /// </param>
    /// <param name="result">
    /// Result of evaluating the condition.
    /// </param>
    /// <returns>
    /// True if the condition was successfully parsed, false otherwise.
    /// </returns>
    bool TryEval(ref OopContext oopContext, ref Word instruction, out bool result);
}