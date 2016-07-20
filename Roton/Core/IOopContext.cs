using Roton.Core;

namespace Roton.Emulation.Execution
{
    public interface IOopContext
    {
        IActor Actor { get; }
        int CommandsExecuted { get; set; }
        ITile DeathTile { get; }
        bool Died { get; set; }
        IElementList Elements { get; }
        bool Finished { get; set; }
        IFlagList Flags { get; }
        IXyPair GetRandomDirection();
        int GetRandomNumber(int max);
        IXyPair GetSeek(IXyPair location);
        IGrammar Grammar { get; }
        int Index { get; set; }
        int Instruction { get; set; }
        string Message { get; set; }
        void MoveActor(int index, IXyPair location);
        bool Moved { get; set; }
        string Name { get; set; }
        bool NextLine { get; set; }
        int OopByte { get; set; }
        int OopNumber { get; set; }
        IActor Player { get; }
        int PreviousInstruction { get; set; }
        void Push(IXyPair location, IXyPair vector);
        void RaiseError(string error);
        int ReadNextNumber();
        string ReadNextWord();
        bool Repeat { get; set; }
        void UpdateBoard(IXyPair location);
        IWorld World { get; }
        IElement ElementAt(IXyPair location);
    }
}