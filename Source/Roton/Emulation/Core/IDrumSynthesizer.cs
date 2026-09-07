using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

/// <summary>
/// Handles the creation of drum frequency tables.
/// </summary>
public interface IDrumSynthesizer
{
    /// <summary>
    /// Populates a drum frequency table.
    /// </summary>
    /// <param name="id">
    /// ID of the drum.
    /// </param>
    /// <param name="buffer">
    /// Temporary buffer to use.
    /// </param>
    /// <remarks>
    /// The first word is the count of frequencies in the table.
    /// The remaining words are frequencies, in hz.
    /// </remarks>
    void Synthesize(int id, Span<Word> buffer);
}