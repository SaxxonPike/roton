using System;
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
    public int Search(int index, ReadOnlySpan<char> term)
    {
        if (term.IsEmpty)
            return -1;

        var termLength = term.Length;
        var code = actors.GetActorCode(index);

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

    public char ReadByte(int index, ref Word instruction)
    {
        var code = actors.GetActorCode(index);
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

    public ReadOnlySpan<char> ReadLine(int index, ref Word instruction, Span<char> buffer)
    {
        var code = actors.GetActorCode(index);
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

    public int ReadNumber(int index, ref Word instruction)
    {
        var code = actors.GetActorCode(index);
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

    public ReadOnlySpan<char> ReadWord(int index, ref Word instruction, Span<char> buffer)
    {
        var code = actors.GetActorCode(index);
        var length = 0;
        int instr = instruction;

        // Skip leading spaces.
        var codeWithoutSpaces = code.Slice(instr).TrimStart(' ');
        instr = code.Length - codeWithoutSpaces.Length;
        var b = instr < code.Length ? code[instr++].ToUpperCase() : '\0';

        // Match a word like this regex: ^[A-Z:_][A-Z0-9:_]*
        if (b is not (>= '0' and <= '9'))
        {
            while (b is >= 'A' and <= 'Z' or >= '0' and <= '9' or ':' or '_')
            {
                if (length < buffer.Length)
                    buffer[length++] = b;
                b = instr < code.Length
                    ? code[instr++].ToUpperCase()
                    : '\0';
            }
        }

        if (instr > 0)
            instr--;

        var result = buffer.Slice(0, length);
        state.SetOopWord(result);
        state.OopByte = b;
        instruction = instr;

        return result;
    }
}