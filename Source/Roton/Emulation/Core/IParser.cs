using Roton.Emulation.Data;
using Roton.Emulation.Items;

namespace Roton.Emulation.Core;

public interface IParser
{
    bool? GetCondition(IOopContext oopContext);
    IXyPair GetDirection(IOopContext oopContext);
    IItem GetItem(IOopContext oopContext);
    ITile GetKind(IOopContext oopContext);
    bool GetTarget(ISearchContext context);
    int ReadByte(int index, IExecutable instructionSource);
    string ReadLine(int index, IExecutable instructionSource);
    int ReadNumber(int index, IExecutable instructionSource);
    string ReadWord(int index, IExecutable instructionSource);
    int Search(int index, int offset, string term);
    int GetNumber(IOopContext context);
}