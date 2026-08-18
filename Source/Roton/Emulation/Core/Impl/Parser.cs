using System;
using System.Diagnostics;
using System.Linq;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Emulation.Items;
using Roton.Infrastructure.Impl;

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

        var termBytes = term.Length <= 256 ? stackalloc byte[term.Length] : new byte[term.Length];
        term.ToBytes(termBytes);
        var actor = Engine.Actors[index];
        var offs = new Word();
        
        while (offs < actor.Length)
        {
            var oldOffset = offs;
            var termOffset = 0;
            bool success;
        
            while (true)
            {
                ReadByte(index, ref offs);
                if (termBytes[termOffset].ToUpperCase() != Engine.State.OopByte.ToUpperCase())
                {
                    success = false;
                    break;
                }
        
                termOffset++;
                if (termOffset >= termBytes.Length)
                {
                    success = true;
                    break;
                }
            }
        
            if (success)
            {
                ReadByte(index, ref offs);
                Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
                if ((int)Engine.State.OopByte is not (>= 0x41 and <= 0x5A or 0x5F))
                {
                    result = oldOffset;
                    break;
                }
            }
        
            oldOffset++;
            offs = oldOffset;
        }
        
        return result;
    }

    public int GetNumber(ref OopContext context, ref Word instruction) => 
        ReadNumber(context.Index, ref instruction);

    public void DiscardLine(int index, ref Word instruction)
    {
        ReadByte(index, ref instruction);
        while (Engine.State.OopByte != 0x00 && Engine.State.OopByte != 0x0D)
            ReadByte(index, ref instruction);
    }

    public int ReadByte(int index, ref Word instruction)
    {
        var actor = Engine.Actors[index];
        var value = 0;

        if (instruction < 0 || instruction >= actor.Length)
        {
            Engine.State.OopByte = 0;
        }
        else
        {
            value = actor.Code[instruction];
            Engine.State.OopByte = value;
            instruction++;
        }

        return value;
    }

    public string ReadLine(int index, ref Word instruction)
    {
        // The original ZZT engine used a string[50] Pascal buffer for OOP line reads.
        // 256 chars is generous headroom while remaining safe for stack allocation (~512 bytes).

        var buffer = (stackalloc char[256]);
        var length = 0;
        ReadByte(index, ref instruction);
        while (Engine.State.OopByte != 0x00 && Engine.State.OopByte != 0x0D)
        {
            if (length < buffer.Length)
                buffer[length++] = Engine.State.OopByte.ToChar();
            ReadByte(index, ref instruction);
        }

        return buffer.Slice(0, length).ToString();
    }

    public int ReadNumber(int index, ref Word instruction)
    {
        var success = false;
        var resultInt = 0;

        while (ReadByte(index, ref instruction) == 0x20)
        {
        }

        Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
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
        Span<char> result = stackalloc char[256];
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

        Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
        var oopByte = Engine.State.OopByte;

        if ((int)oopByte is not (>= 0x30 and <= 0x39))
        {
            while ((int)oopByte is >= 0x41 and <= 0x5A or >= 0x30 and <= 0x39 or 0x3A or 0x5F)
            {
                if (length < buffer.Length)
                    buffer[length++] = oopByte.ToChar();
                ReadByte(index, ref instruction);
                Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
                oopByte = Engine.State.OopByte;
            }
        }

        if (instruction > 0) 
            instruction--;

        var result = buffer.Slice(0, length);
        Engine.State.SetOopWord(result);
        return result;
    }

    public bool? GetCondition(ref OopContext oopContext, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[256];
        var name = ReadWord(oopContext.Index, ref instruction, buffer);
        var condition = Engine.ConditionList.Get(name);
        return condition?.Execute(ref oopContext, ref instruction) ?? Engine.World.Flags.Contains(name);
    }

    public Vector? GetDirection(ref OopContext oopContext, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[256];
        var name = ReadWord(oopContext.Index, ref instruction, buffer);
        var direction = Engine.DirectionList.Get(name);
        return direction?.Execute(ref oopContext, ref instruction);
    }

    public IItem GetItem(ref OopContext oopContext, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[256];
        var name = ReadWord(oopContext.Index, ref instruction, buffer);
        var item = Engine.ItemList.Get(name);
        return item;
    }

    public Tile? GetKind(ref OopContext oopContext, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[256];
        var word = ReadWord(oopContext.Index, ref instruction, buffer);
        var result = new Tile(0, 0);
        var success = false;

        var colorId = Engine.Colors.IndexOf(word);
        if (colorId > 0)
        {
            result.Color = colorId + 8;
            word = ReadWord(oopContext.Index, ref instruction, buffer);
        }
        
        var elementId = Engine.ElementList.IndexOf(word);
        if (elementId >= 0)
        {
            success = true;
            result.Id = elementId;
        }

        return success ? result : null;
    }

    public bool GetTarget(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        context.Index++;
        var target = Engine.TargetList.Get(term) ?? Engine.TargetList.Get(string.Empty);
        return target.Execute(index, ref context, term);
    }
}