using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.Execution
{
    public interface IParser
    {
        bool? GetCondition(IOopContext oopContext);
        IXyPair GetDirection(IOopContext oopContext);
        IOopItem GetItem(IOopContext oopContext);
        ITile GetKind(IOopContext oopContext);
        bool GetTarget(ISearchContext context);
        int ReadByte(int index, IExecutable instructionSource);
        string ReadLine(int index, IExecutable instructionSource);
        int ReadNumber(int index, IExecutable instructionSource);
        string ReadWord(int index, IExecutable instructionSource);
        int Search(int index, string term);
        int GetNumber(IOopContext context);
    }
}