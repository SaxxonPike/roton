using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

/// <summary>
/// Handles parsing tokens from a script.
/// </summary>
public interface IParser
{
    /// <summary>
    /// Reads the next byte from the script into OopByte.
    /// </summary>
    /// <param name="index">
    ///     Index of the actor that contains the script.
    /// </param>
    /// <param name="instruction">
    ///     Instruction counter.
    /// </param>
    /// <returns>
    /// The byte that was read or -1 if unsuccessful.
    /// </returns>
    char ReadByte(int index, ref Word instruction);

    /// <summary>
    /// Reads the next line from the script into a temporary buffer.
    /// </summary>
    /// <param name="index">
    /// Index of the actor that contains the script.
    /// </param>
    /// <param name="instruction">
    /// Instruction counter.
    /// </param>
    /// <param name="buffer">
    /// A temporary buffer to store the line that was read.
    /// </param>
    /// <returns>
    /// A span within the temporary buffer that contains the line that was read.
    /// </returns>
    ReadOnlySpan<char> ReadLine(int index, ref Word instruction, Span<char> buffer);

    /// <summary>
    /// Reads the next number from the script into OopNumber.
    /// </summary>
    /// <param name="index">
    /// Index of the actor that contains the script.
    /// </param>
    /// <param name="instruction">
    /// Instruction counter.
    /// </param>
    /// <returns>
    /// The number that was read or -1 if unsuccessful.
    /// </returns>
    int ReadNumber(int index, ref Word instruction);

    /// <summary>
    /// Reads the next word from the script into OopWord.
    /// </summary>
    /// <param name="index">
    /// Index of the actor that contains the script.
    /// </param>
    /// <param name="instruction">
    /// Instruction counter.
    /// </param>
    /// <param name="buffer">
    /// A temporary buffer to store the word that was read.
    /// </param>
    /// <returns>
    /// A span within the temporary buffer that contains the word that was read.
    /// </returns>
    /// <remarks>
    /// RoZ: OopReadWord
    /// </remarks>
    ReadOnlySpan<char> ReadWord(int index, ref Word instruction, Span<char> buffer);

    /// <summary>
    /// Searches for a term in the script.
    /// </summary>
    /// <param name="index">
    /// Index of the actor that contains the script.
    /// </param>
    /// <param name="term">
    /// The term to search for.
    /// </param>
    /// <returns>
    /// The index of the term in the script, or -1 if the term was not found.
    /// </returns>
    int Search(int index, ReadOnlySpan<char> term);
}