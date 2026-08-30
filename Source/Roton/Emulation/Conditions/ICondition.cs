using Roton.Emulation.Data;

namespace Roton.Emulation.Conditions;

/// <summary>
/// Represents a boolean condition that can be evaluated during script execution.
/// </summary>
public interface ICondition
{
    /// <summary>
    /// Evaluates a boolean condition for the specified script context.
    /// </summary>
    /// <param name="context">
    /// Script context.
    /// </param>
    /// <param name="instruction">
    /// Current offset within the script, which may be modified by the condition.
    /// </param>
    /// <returns>
    /// The result of evaluating the boolean condition.
    /// </returns>
    bool? Execute(ref OopContext context, ref Word instruction);
}