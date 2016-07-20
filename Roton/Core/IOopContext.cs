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
        bool Moved { get; set; }
        string Name { get; set; }
        bool NextLine { get; set; }
        IActor Player { get; }
        int PreviousInstruction { get; set; }
        int ReadNextNumber();
        string ReadNextWord();
        bool Repeat { get; set; }
        IWorld World { get; }
    }
}