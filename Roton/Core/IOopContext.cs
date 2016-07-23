namespace Roton.Core
{
    public interface IOopContext
    {
        IActor Actor { get; }
        ITile DeathTile { get; }
        IElementList Elements { get; }
        IFlagList Flags { get; }
        IGrammar Grammar { get; }
        IActor Player { get; }
        IWorld World { get; }
        int CommandsExecuted { get; set; }
        bool Died { get; set; }
        bool Finished { get; set; }
        int Index { get; set; }
        int Instruction { get; set; }
        string Message { get; set; }
        bool Moved { get; set; }
        string Name { get; set; }
        bool NextLine { get; set; }
        int OopByte { get; set; }
        int OopNumber { get; set; }
        int PreviousInstruction { get; set; }
        bool Repeat { get; set; }
        IElement ElementAt(IXyPair location);
        IXyPair GetRandomDirection();
        int GetRandomNumber(int max);
        IXyPair GetSeek(IXyPair location);
        void MoveActor(int index, IXyPair location);
        void Push(IXyPair location, IXyPair vector);
        void RaiseError(string error);
        int ReadNextNumber();
        string ReadNextWord();
        void UpdateBoard(IXyPair location);
    }
}