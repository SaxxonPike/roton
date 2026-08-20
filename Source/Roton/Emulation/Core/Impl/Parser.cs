using System;
using System.Diagnostics;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Items;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Parser(IEngineAccessor engine) : IParser
{
    private IEngine Engine
    {
        [DebuggerStepThrough] get => engine.Instance;
    }

    private ReadOnlySpan<char> GetActorCode(int index)
    {
        var actor = Engine.Actors[index];
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

            Engine.State.OopByte = endChar;
            return startIdx;
        }

        return -1;
    }

    public char ReadByte(int index, ref Word instruction)
    {
        var actor = Engine.Actors[index];
        var value = '\0';

        if (instruction < 0 || instruction >= actor.Length)
        {
            Engine.State.OopByte = default;
        }
        else
        {
            value = actor.Code.Span[instruction];
            Engine.State.OopByte = value;
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

        Engine.State.OopByte = b;
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
        Engine.State.OopByte = b.ToUpperCase();

        if (!success)
            Engine.State.OopNumber = -1;
        else
            Engine.State.OopNumber = resultInt;

        return Engine.State.OopNumber;
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
        Engine.State.SetOopWord(result);
        Engine.State.OopByte = b;
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

        var condition = Engine.Conditions.Get(name);
        result = condition?.Execute(ref oopContext, ref instruction) ?? Engine.World.Flags.Contains(name);
        return true;
    }

    public bool TryEvalDirection(ref OopContext oopContext, ref Word instruction, out Vector result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var name = ReadWord(oopContext.Index, ref instruction, buffer);
        var direction = Engine.Directions.Get(name);

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
        result = Engine.ItemList.Get(name);
        return result != null;
    }

    public bool TryEvalKind(ref OopContext oopContext, ref Word instruction, out Tile result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var word = ReadWord(oopContext.Index, ref instruction, buffer);
        var success = false;
        result = new Tile(0, 0);

        var colorId = Engine.Colors.IndexOf(word);
        if (colorId > 0)
        {
            result.Color = colorId + 8;
            word = ReadWord(oopContext.Index, ref instruction, buffer);
        }

        var elementId = Engine.Elements.IndexOf(word);
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
        var target = Engine.TargetList.Get(term) ?? Engine.TargetList.Get(string.Empty);
        return target?.Execute(index, ref context, term) ?? false;
    }
}