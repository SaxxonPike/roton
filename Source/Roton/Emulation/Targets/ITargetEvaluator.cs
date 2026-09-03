using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Targets;

public interface ITargetEvaluator
{
    /// <summary>
    /// Reads a target name from the script.
    /// </summary>
    /// <param name="index">
    /// Index of the actor that contains the script.
    /// </param>
    /// <param name="context">
    /// Execution context.
    /// </param>
    /// <param name="term">
    /// A temporary buffer that contains the word read from the script.
    /// </param>
    /// <returns>
    /// True if the target was successfully parsed, false otherwise.
    /// </returns>
    bool TryEval(int index, ref SearchContext context, ReadOnlySpan<char> term);
}