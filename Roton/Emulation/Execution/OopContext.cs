using Roton.Core;

namespace Roton.Emulation.Execution
{
    internal class OopContext : ICodeInstruction, IOopContext
    {
        private int _index;
        private readonly ICodeInstruction _instructionSource;

        public OopContext(
            int index,
            ICodeInstruction instructionSource,
            string name,
            ICore core)
        {
            _instructionSource = instructionSource;
            _index = index;
            Index = index;
            Name = name;
            Core = core;
            DeathTile = new Tile(0, 0);
        }

        public IActor Actor => Core.Actors[_index];

        public int CommandsExecuted { get; set; }

        private ICore Core { get; }

        public ITile DeathTile { get; }

        public bool Died { get; set; }

        public IElementList Elements => Core.Elements;

        public bool Finished { get; set; }

        public IFlagList Flags => Core.Flags;

        public IXyPair GetRandomDirection() => Core.Rnd();

        public int GetRandomNumber(int max) => Core.RandomNumber(max);

        public IXyPair GetSeek(IXyPair location) => Core.Seek(location);

        public IGrammar Grammar => Core.Grammar;

        public int Index { get; set; }

        public int Instruction
        {
            get { return _instructionSource.Instruction; }
            set { _instructionSource.Instruction = value; }
        }

        public string Message { get; set; }

        public bool Moved { get; set; }

        public string Name { get; set; }

        public bool NextLine { get; set; }

        public IActor Player => Core.Player;

        public int PreviousInstruction { get; set; }

        public int ReadNextNumber() => Core.ReadActorCodeNumber(Index, _instructionSource);

        public string ReadNextWord()
        {
            throw new System.NotImplementedException();
        }

        public bool Repeat { get; set; }

        public IWorld World => Core.WorldData;
    }
}