using Roton.Core;

namespace Roton.Emulation.Execution
{
    internal class OopContext : ICodeInstruction, IOopContext
    {
        private readonly int _index;
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

        public void MoveActor(int index, IXyPair location) => Core.MoveActor(index, location);

        public bool Moved { get; set; }

        public string Name { get; set; }

        public bool NextLine { get; set; }

        public int OopByte
        {
            get { return Core.OopByte; }
            set { Core.OopByte = value; }
        }

        public int OopNumber
        {
            get { return Core.OopNumber; }
            set { Core.OopNumber = value; }
        }

        public IActor Player => Core.Player;

        public int PreviousInstruction { get; set; }

        public void Push(IXyPair location, IXyPair vector) => Core.Push(location, vector);

        public void RaiseError(string error)
        {
            Core.SetMessage(0xC8, $"ERR: {error}");
            Core.PlaySound(5, Core.Sounds.Error);
        }

        public int ReadNextNumber() => Core.ReadActorCodeNumber(Index, _instructionSource);

        public string ReadNextWord() => Core.ReadActorCodeWord(Index, _instructionSource);

        public bool Repeat { get; set; }

        public void UpdateBoard(IXyPair location) => Core.UpdateBoard(location);

        public IWorld World => Core.WorldData;

        public IElement ElementAt(IXyPair location) => Core.ElementAt(location);
    }
}