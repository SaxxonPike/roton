using System;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Items;

namespace Roton.Emulation.Core;

/// <summary>
/// Handles parsing tokens from a script.
/// </summary>
public interface IParser
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
    bool TryEvalCondition(ref OopContext oopContext, ref Word instruction, out bool result);

    /// <summary>
    /// Reads a direction name from the script, then converts it to a vector.
    /// </summary>
    /// <param name="oopContext">
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
    bool TryEvalDirection(ref OopContext oopContext, ref Word instruction, out Vector result);

    /// <summary>
    /// Reads an item name from the script.
    /// </summary>
    /// <param name="oopContext">
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
    bool TryEvalItem(ref OopContext oopContext, ref Word instruction, out IItem? result);

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
    bool TryEvalKind(ref OopContext oopContext, ref Word instruction, out Tile result);

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
    bool TryEvalTarget(int index, ref SearchContext context, ReadOnlySpan<char> term);

    /// <summary>
    /// Reads the next byte from the script into OopByte.
    /// </summary>
    /// <param name="index">
    /// Index of the actor that contains the script.
    /// </param>
    /// <param name="instruction">
    /// Instruction counter.
    /// </param>
    /// <returns>
    /// The byte that was read or -1 if unsuccessful.
    /// </returns>
    int ReadByte(int index, ref Word instruction);

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