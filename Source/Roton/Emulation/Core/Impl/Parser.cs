using System;
using System.Diagnostics;
using System.Linq;
using Roton.Emulation.Data;
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
        var offs = new Executable();
        
        while (offs.Instruction < actor.Length)
        {
            var oldOffset = offs.Instruction;
            var termOffset = 0;
            bool success;
        
            while (true)
            {
                ReadByte(index, offs);
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
                ReadByte(index, offs);
                Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
                if (!(Engine.State.OopByte is >= 0x41 and <= 0x5A or 0x5F))
                {
                    result = oldOffset;
                    break;
                }
            }
        
            oldOffset++;
            offs.Instruction = oldOffset;
        }
        
        return result;
    }

    public int GetNumber(IOopContext context) => 
        ReadNumber(context.Index, context);

    public void DiscardLine(int index, IExecutable instructionSource)
    {
        ReadByte(index, instructionSource);
        while (Engine.State.OopByte != 0x00 && Engine.State.OopByte != 0x0D)
            ReadByte(index, instructionSource);
    }

    public int ReadByte(int index, IExecutable instructionSource)
    {
        var actor = Engine.Actors[index];
        var value = 0;

        if (instructionSource.Instruction < 0 || instructionSource.Instruction >= actor.Length)
        {
            Engine.State.OopByte = 0;
        }
        else
        {
            value = actor.Code[instructionSource.Instruction];
            Engine.State.OopByte = value;
            instructionSource.Instruction++;
        }

        return value;
    }

    public string ReadLine(int index, IExecutable instructionSource)
    {
        // The original ZZT engine used a string[50] Pascal buffer for OOP line reads.
        // 256 chars is generous headroom while remaining safe for stack allocation (~512 bytes).

        var buffer = (stackalloc char[256]);
        var length = 0;
        ReadByte(index, instructionSource);
        while (Engine.State.OopByte != 0x00 && Engine.State.OopByte != 0x0D)
        {
            if (length < buffer.Length)
                buffer[length++] = Engine.State.OopByte.ToChar();
            ReadByte(index, instructionSource);
        }

        return buffer.Slice(0, length).ToString();
    }

    public int ReadNumber(int index, IExecutable instructionSource)
    {
        var success = false;
        var resultInt = 0;

        while (ReadByte(index, instructionSource) == 0x20)
        {
        }

        Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
        while (Engine.State.OopByte is >= 0x30 and <= 0x39)
        {
            success = true;
            resultInt = resultInt * 10 + (Engine.State.OopByte - 0x30);
            ReadByte(index, instructionSource);
        }

        if (instructionSource.Instruction > 0)
        {
            instructionSource.Instruction--;
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

    public void ReadWord(int index, IExecutable instructionSource)
    {
        Span<char> result = stackalloc char[256];
        ReadWord(index, instructionSource, result);
    }

    public ReadOnlySpan<char> ReadWord(int index, IExecutable instructionSource, Span<char> buffer)
    {
        var length = 0;

        while (true)
        {
            ReadByte(index, instructionSource);
            if (Engine.State.OopByte != 0x20)
            {
                break;
            }
        }

        Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
        var oopByte = Engine.State.OopByte;

        if (oopByte is not (>= 0x30 and <= 0x39))
        {
            while (oopByte is >= 0x41 and <= 0x5A or >= 0x30 and <= 0x39 or 0x3A or 0x5F)
            {
                if (length < buffer.Length)
                    buffer[length++] = oopByte.ToChar();
                ReadByte(index, instructionSource);
                Engine.State.OopByte = Engine.State.OopByte.ToUpperCase();
                oopByte = Engine.State.OopByte;
            }
        }

        if (instructionSource.Instruction > 0) 
            instructionSource.Instruction--;

        var result = buffer.Slice(0, length);
        Engine.State.SetOopWord(result);
        return result;
    }

    public bool? GetCondition(IOopContext oopContext)
    {
        Span<char> buffer = stackalloc char[256];
        var name = ReadWord(oopContext.Index, oopContext, buffer);
        var condition = Engine.ConditionList.Get(name);
        return condition?.Execute(oopContext) ?? Engine.World.Flags.Contains(name);
    }

    public IXyPair GetDirection(IOopContext oopContext)
    {
        Span<char> buffer = stackalloc char[256];
        var name = ReadWord(oopContext.Index, oopContext, buffer);
        var direction = Engine.DirectionList.Get(name);
        return direction?.Execute(oopContext);
    }

    public IItem GetItem(IOopContext oopContext)
    {
        Span<char> buffer = stackalloc char[256];
        var name = ReadWord(oopContext.Index, oopContext, buffer);
        var item = Engine.ItemList.Get(name);
        return item;
    }

    public ITile GetKind(IOopContext oopContext)
    {
        Span<char> buffer = stackalloc char[256];
        var word = ReadWord(oopContext.Index, oopContext, buffer);
        var result = new Tile(0, 0);
        var success = false;

        for (var i = 1; i < 8; i++)
        {
            if (!Engine.Colors[i].CaseInsensitiveEqual(word))
                continue;

            result.Color = i + 8;
            word = ReadWord(oopContext.Index, oopContext, buffer);
            break;
        }

        foreach (var element in Engine.ElementList.Where(e => e != null))
        {
            if (!element.Name.CaseInsensitiveCharacterEqual(word))
                continue;

            success = true;
            result.Id = element.Id;
            break;
        }

        return success ? result : null;
    }

    public bool GetTarget(int index, ISearchContext context, ReadOnlySpan<char> term)
    {
        context.SearchIndex++;
        var target = Engine.TargetList.Get(term) ?? Engine.TargetList.Get(string.Empty);
        return target.Execute(index, context, term);
    }
}