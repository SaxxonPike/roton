using System;
#if NET10_0_OR_GREATER
using System.Buffers;
#endif
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Parser(
    IActorList actors,
    IState state)
    : IParser
{
#if NET10_0_OR_GREATER
    private static readonly SearchValues<char> WordChars =
        SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789:_");
#endif

    /// <inheritdoc />
    public int Search(int index, ReadOnlySpan<char> term)
    {
        if (term.IsEmpty)
            return -1;

        var termLength = term.Length;
        var code = actors.GetCode(index);

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
    public char ReadByte(int index, ref Word instruction)
    {
        var code = actors.GetCode(index);
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
    public ReadOnlySpan<char> ReadLine(int index, ref Word instruction, Span<char> buffer)
    {
        var code = actors.GetCode(index);
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
    public int ReadNumber(int index, ref Word instruction)
    {
        var code = actors.GetCode(index);
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
    public ReadOnlySpan<char> ReadWord(int index, ref Word instruction, Span<char> buffer)
    {
        var code = actors.GetCode(index);
        int instr = instruction;

        if (instr >= code.Length)
        {
            if (instr > 0)
                instr--;
            state.OopByte = '\0';
            state.SetOopWord([]);
            instruction = instr;
            return [];
        }

        // Skip leading spaces.
        var remaining = code.Slice(instr);
#if NET10_0_OR_GREATER
        var spaceSkip = remaining.IndexOfAnyExcept(' ');
#else
        var spaceSkip = remaining.Length - remaining.TrimStart(' ').Length;
#endif
        if (spaceSkip < 0)
        {
            instr = code.Length;
            if (instr > 0)
                instr--;
            state.OopByte = '\0';
            state.SetOopWord([]);
            instruction = instr;
            return [];
        }

        instr += spaceSkip;
        remaining = code.Slice(instr);

        var firstChar = remaining[0];
        // Word cannot start with a digit
        if (firstChar is >= '0' and <= '9')
        {
            state.OopByte = firstChar.ToUpperCase();
            state.SetOopWord([]);
            instruction = instr;
            return [];
        }

#if NET10_0_OR_GREATER
        var wordLen = remaining.IndexOfAnyExcept(WordChars);
#else
        var wordLen = 0;
        while (wordLen < remaining.Length)
        {
            var ch = remaining[wordLen];
            if (ch is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or ':' or '_')
                wordLen++;
            else
                break;
        }
#endif
        if (wordLen < 0)
            wordLen = remaining.Length;

        var rawWord = remaining.Slice(0, wordLen);
        instr += wordLen;

        var nextChar = instr < code.Length ? code[instr].ToUpperCase() : '\0';

        var length = Math.Min(rawWord.Length, buffer.Length);
        for (var i = 0; i < length; i++)
            buffer[i] = rawWord[i].ToUpperCase();

        var result = buffer.Slice(0, length);
        state.SetOopWord(result);
        state.OopByte = nextChar;
        instruction = instr;

        return result;
    }
}