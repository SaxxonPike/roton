using System;
using System.Diagnostics;
using Roton.Emulation.Conditions;
using Roton.Emulation.Data;
using Roton.Emulation.Directions;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Items;
using Roton.Emulation.Targets;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Parser(
    IEngineAccessor engine,
    IActorList actorList,
    IState state,
    IConditionList conditionList,
    IDirectionList directionList,
    IItemList itemList,
    IColorList colorList,
    IFlags flags,
    IElementList elementList,
    ITargetList targetList)
    : IParser
{
    private IEngine Engine
    {
        [DebuggerStepThrough] get => engine.Instance;
    }

    private ReadOnlySpan<char> GetActorCode(int index)
    {
        var actor = actorList[index];
        var codeLength = Math.Min(Math.Max(0, (int)actor.Length), actor.Code.Length);
        return actor.Code.Span.Slice(0, codeLength);
    }

    public int Search(int index, ReadOnlySpan<char> term)
    {
        var result = -1;
        if (term.IsEmpty)
            return result;

        var termLength = term.Length;
        var code = GetActorCode(index);

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
        var actor = actorList[index];
        var value = '\0';

        if (instruction < 0 || instruction >= actor.Length)
        {
            state.OopByte = default;
        }
        else
        {
            value = actor.Code.Span[instruction];
            state.OopByte = value;
            instruction++;
        }

        return value;
    }

    public ReadOnlySpan<char> ReadLine(int index, ref Word instruction, Span<char> buffer)
    {
        var code = GetActorCode(index);
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
        var code = GetActorCode(index);
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
        var length = 0;
        var code = GetActorCode(index);
        int instr = instruction;
        var b = '\0';

        while (instr < code.Length)
        {
            b = code[instr++];
            if (b != ' ')
                break;
        }

        b = b.ToUpperCase();

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

    public bool TryEvalCondition(ref OopContext oopContext, ref Word instruction, out bool result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var name = ReadWord(oopContext.Index, ref instruction, buffer);

        if (name.IsEmpty)
        {
            result = false;
            return false;
        }

        var condition = conditionList.Get(name);
        result = condition?.Execute(ref oopContext, ref instruction) ?? flags.Contains(name);
        return true;
    }

    public bool TryEvalDirection(ref OopContext oopContext, ref Word instruction, out Vector result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var name = ReadWord(oopContext.Index, ref instruction, buffer);
        var direction = directionList.Get(name);

        if (direction?.Execute(ref oopContext, ref instruction) is not { } temp)
        {
            result = default;
            return false;
        }

        result = temp;
        return true;
    }

    public bool TryEvalItem(ref OopContext oopContext, ref Word instruction, out IItem? result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var name = ReadWord(oopContext.Index, ref instruction, buffer);
        result = itemList.Get(name);
        return result != null;
    }

    public bool TryEvalKind(ref OopContext oopContext, ref Word instruction, out Tile result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var word = ReadWord(oopContext.Index, ref instruction, buffer);
        var success = false;
        result = new Tile(0, 0);

        var colorId = colorList.IndexOf(word);
        if (colorId > 0)
        {
            result.Color = colorId + 8;
            word = ReadWord(oopContext.Index, ref instruction, buffer);
        }

        var elementId = elementList.IndexOf(word);
        if (elementId >= 0)
        {
            success = true;
            result.Id = elementId;
        }

        return success;
    }

    public bool TryEvalTarget(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        context.Index++;
        var target = targetList.Get(term) ?? targetList.Get(string.Empty);
        return target?.Execute(index, ref context, term) ?? false;
    }
}