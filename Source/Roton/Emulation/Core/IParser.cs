using System;
using Roton.Emulation.Data;
using Roton.Emulation.Items;

namespace Roton.Emulation.Core;

public interface IParser
{
    bool? GetCondition(IOopContext oopContext);
    IXyPair GetDirection(IOopContext oopContext);
    IItem GetItem(IOopContext oopContext);
    ITile GetKind(IOopContext oopContext);
    bool GetTarget(int index, ISearchContext context, ReadOnlySpan<char> term);
    int ReadByte(int index, IExecutable instructionSource);
    string ReadLine(int index, IExecutable instructionSource);
    int ReadNumber(int index, IExecutable instructionSource);
    void ReadWord(int index, IExecutable instructionSource);
    ReadOnlySpan<char> ReadWord(int index, IExecutable instructionSource, Span<char> buffer);
    int Search(int index, ReadOnlySpan<char> term);
    int GetNumber(IOopContext context);
    void DiscardLine(int index, IExecutable instructionSource);
}