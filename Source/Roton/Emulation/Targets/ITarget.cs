using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Targets;

/// <summary>
/// Represents a target resolver for labels.
/// </summary>
public interface ITarget
{
    /// <summary>
    /// Evaluates the specified label target candidate.
    /// </summary>
    /// <param name="index">
    /// Index of the candidate.
    /// </param>
    /// <param name="context">
    /// Candidate search context.
    /// </param>
    /// <param name="term">
    /// Name of the label.
    /// </param>
    /// <returns>
    /// True if the candidate is a match, otherwise false.
    /// </returns>
    bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term);
}