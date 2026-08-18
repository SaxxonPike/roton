using System;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Items;

namespace Roton.Emulation.Core;

public interface IParser
{
    bool? GetCondition(ref OopContext oopContext, ref Word instruction);
    Vector? GetDirection(ref OopContext oopContext, ref Word instruction);
    IItem GetItem(ref OopContext oopContext, ref Word instruction);
    Tile? GetKind(ref OopContext oopContext, ref Word instruction);
    bool GetTarget(int index, ref SearchContext context, ReadOnlySpan<char> term);
    int ReadByte(int index, ref Word instruction);
    string ReadLine(int index, ref Word instruction);
    ReadOnlySpan<char> ReadLine(int index, ref Word instruction, Span<char> buffer);
    int ReadNumber(int index, ref Word instruction);
    void ReadWord(int index, ref Word instruction);
    ReadOnlySpan<char> ReadWord(int index, ref Word instruction, Span<char> buffer);
    int Search(int index, ReadOnlySpan<char> term);
    int GetNumber(ref OopContext context, ref Word instruction);
    void DiscardLine(int index, ref Word instruction);
}