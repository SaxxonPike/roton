using System;
using System.Runtime.CompilerServices;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Parser(IState state)
    : IParser
{

    /// <inheritdoc />
    public int Search(ReadOnlySpan<char> code, ReadOnlySpan<char> term)
    {
        if (term.IsEmpty)
            return -1;

        var termLength = term.Length;

        var startIdx = 0;

        while (startIdx < code.Length)
        {
            var foundIdx = code
                .Slice(startIdx)
                .IndexOf(term, StringComparison.OrdinalIgnoreCase);

            if (foundIdx < 0)
                break;

            startIdx += foundIdx;

            var endIdx = startIdx + termLength;

            var endChar = endIdx >= code.Length
                ? '\0'
                : code[endIdx].ToUpperCase();

            if (endChar is '_' or >= 'A' and <= 'Z')
            {
                startIdx++;
                continue;
            }

            state.OopByte = endChar;
            return startIdx;
        }

        return -1;
    }

    /// <inheritdoc />
    public char ReadByte(ReadOnlySpan<char> code, ref Word instruction)
    {
        var value = '\0';

        if (instruction < 0 || instruction >= code.Length)
        {
            state.OopByte = default;
        }
        else
        {
            value = code[instruction];
            state.OopByte = value;
            instruction++;
        }

        return value;
    }

    /// <inheritdoc />
    public ReadOnlySpan<char> ReadLine(ReadOnlySpan<char> code, ref Word instruction, Span<char> buffer)
    {
        var length = 0;
        int instr = instruction;

        var b = instr < code.Length
            ? code[instr++]
            : '\0';

        while (b != '\0' && b != '\r')
        {
            if (length < buffer.Length)
                buffer[length++] = b;
            b = instr < code.Length
                ? code[instr++]
                : '\0';
        }

        instruction = instr;

        state.OopByte = b;
        return buffer.Slice(0, length);
    }

    /// <inheritdoc />
    public int ReadNumber(ReadOnlySpan<char> code, ref Word instruction)
    {
        var success = false;
        var resultInt = 0;
        int instr = instruction;
        var b = '\0';

        // Skip spaces.

        while (instr < code.Length)
        {
            b = code[instr++];
            if (b != ' ')
                break;
        }

        if (instr >= code.Length)
            b = '\0';

        while (b is >= '0' and <= '9')
        {
            success = true;
            resultInt = resultInt * 10 + (b - 0x30);
            b = instr < code.Length
                ? code[instr++]
                : '\0';
        }

        if (instr > 0) 
            instr--;

        instruction = instr;
        state.OopByte = b.ToUpperCase();

        if (!success)
            state.OopNumber = -1;
        else
            state.OopNumber = resultInt;

        return state.OopNumber;
    }

    /// <inheritdoc />
    public ReadOnlySpan<char> ReadWord(ReadOnlySpan<char> code, ref Word instruction, Span<char> buffer)
    {
        int instr = instruction;
        var codeLen = code.Length;

        if (instr >= codeLen)
        {
            if (instr > 0)
                instr--;
            state.OopByte = '\0';
            state.SetOopWord([]);
            instruction = instr;
            return [];
        }

        // Skip leading spaces.
        while (instr < codeLen && code[instr] == ' ') 
            instr++;

        if (instr >= codeLen)
        {
            state.OopByte = '\0';
            state.SetOopWord([]);
            instruction = codeLen > 0 ? codeLen - 1 : 0;
            return [];
        }

        var ch = code[instr];

        // Word cannot start with a digit.
        if (ch is >= '0' and <= '9')
        {
            state.OopByte = FastToUpper(ch);
            state.SetOopWord([]);
            instruction = instr;
            return [];
        }

        // Copy uppercase characters into the buffer.
        var bufferLen = buffer.Length;
        var wordLen = 0;

        while (instr < codeLen)
        {
            ch = code[instr];
            if (!IsWordChar(ch))
                break;

            if (wordLen < bufferLen)
                buffer[wordLen++] = FastToUpper(ch);

            instr++;
        }

        var nextChar = instr < codeLen ? FastToUpper(code[instr]) : '\0';
        var result = buffer.Slice(0, wordLen);

        // Sync state.
        state.SetOopWord(result);
        state.OopByte = nextChar;
        instruction = instr;

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char FastToUpper(char c) =>
        unchecked(c is >= 'a' and <= 'z' ? (char)(c - 32) : c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsWordChar(char c) =>
        c is >= 'a' and <= 'z'
            or >= 'A' and <= 'Z'
            or >= '0' and <= '9'
            or ':' or '_';
}