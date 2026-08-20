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

    public int Search(int index, ReadOnlySpan<char> term)
    {
        var result = -1;
        if (term.IsEmpty)
            return result;

        var termLength = term.Length;
        var actor = Engine.Actors[index];
        var codeLength = Math.Min(Math.Max(0, (int)actor.Length), actor.Code.Length);
        var code = actor.Code.Span.Slice(0, codeLength);

        var startIdx = 0;

        while (startIdx < codeLength)
        {
            var foundIdx = code
                .Slice(startIdx)
                .IndexOf(term, StringComparison.OrdinalIgnoreCase);

            if (foundIdx < 0)
                break;

            startIdx += foundIdx;

            var endIdx = startIdx + termLength;

            var endChar = endIdx >= codeLength
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
        var length = 0;

        ReadByte(index, ref instruction);

        while (Engine.State.OopByte != 0x00 && Engine.State.OopByte != 0x0D)
        {
            if (length < buffer.Length)
                buffer[length++] = Engine.State.OopByte;
            ReadByte(index, ref instruction);
        }

        return buffer.Slice(0, length);
    }

    public int ReadNumber(int index, ref Word instruction)
    {
        var success = false;
        var resultInt = 0;

        while (ReadByte(index, ref instruction) == 0x20)
        {
        }

        Engine.State.OopByte = Engine.State.OopByte.ToUpper();
        while ((int)Engine.State.OopByte is >= 0x30 and <= 0x39)
        {
            success = true;
            resultInt = resultInt * 10 + (Engine.State.OopByte - 0x30);
            ReadByte(index, ref instruction);
        }

        if (instruction > 0)
        {
            instruction--;
        }

        if (!success)
        {
            Engine.State.OopNumber = -1;
        }
        else
        {
            Engine.State.OopNumber = resultInt;
        }

        return Engine.State.OopNumber;
    }

    public void ReadWord(int index, ref Word instruction)
    {
        Span<char> result = stackalloc char[byte.MaxValue];
        ReadWord(index, ref instruction, result);
    }

    public ReadOnlySpan<char> ReadWord(int index, ref Word instruction, Span<char> buffer)
    {
        var length = 0;

        while (true)
        {
            ReadByte(index, ref instruction);
            if (Engine.State.OopByte != 0x20)
            {
                break;
            }
        }

        Engine.State.OopByte = Engine.State.OopByte.ToUpper();
        var oopByte = Engine.State.OopByte;

        if ((int)oopByte is not (>= 0x30 and <= 0x39))
        {
            while ((int)oopByte is >= 0x41 and <= 0x5A or >= 0x30 and <= 0x39 or 0x3A or 0x5F)
            {
                if (length < buffer.Length)
                    buffer[length++] = oopByte;
                ReadByte(index, ref instruction);
                Engine.State.OopByte = Engine.State.OopByte.ToUpper();
                oopByte = Engine.State.OopByte;
            }
        }

        if (instruction > 0)
            instruction--;

        var result = buffer.Slice(0, length);
        Engine.State.SetOopWord(result);
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

        if (direction?.Execute(ref oopContext, ref instruction) is not {} temp)
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