using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Execution
{
    internal class OopContext : IExecutable, IOopContext
    {
        private readonly int _index;
        private readonly IExecutable _instructionSource;

        public OopContext(
            int index,
            IExecutable instructionSource,
            string name,
            IEngine engine)
        {
            _instructionSource = instructionSource;
            _index = index;
            Index = index;
            Name = name;
            Engine = engine;
            DeathTile = new Tile(0, 0);
        }

        private IEngine Engine { get; }

        public int Instruction
        {
            get { return _instructionSource.Instruction; }
            set { _instructionSource.Instruction = value; }
        }

        public IActor Actor => Engine.Actors[_index];

        public int CommandsExecuted { get; set; }

        public ITile DeathTile { get; }

        public bool Died { get; set; }

        public IElement ElementAt(IXyPair location) => Engine.ElementAt(location);

        public IElementList Elements => Engine.Elements;

        public bool Finished { get; set; }

        public IFlagList Flags => Engine.World.Flags;

        public IXyPair GetRandomDirection() => Engine.Rnd();

        public int GetRandomNumber(int max) => Engine.RandomNumber(max);

        public IXyPair GetSeek(IXyPair location) => Engine.Seek(location);

        public IGrammar Grammar => Engine.Grammar;

        public int Index { get; set; }

        public string Message { get; set; }

        public void MoveActor(int index, IXyPair location) => Engine.MoveActor(index, location);

        public bool Moved { get; set; }

        public string Name { get; set; }

        public bool NextLine { get; set; }

        public int OopByte
        {
            get { return Engine.State.OopByte; }
            set { Engine.State.OopByte = value; }
        }

        public int OopNumber
        {
            get { return Engine.State.OopNumber; }
            set { Engine.State.OopNumber = value; }
        }

        public IActor Player => Engine.Player;

        public int PreviousInstruction { get; set; }

        public void Push(IXyPair location, IXyPair vector) => Engine.Push(location, vector);

        public void RaiseError(string error) => Engine.RaiseError(error);

        public int ReadNextNumber() => Engine.ReadActorCodeNumber(Index, _instructionSource);

        public string ReadNextWord() => Engine.ReadActorCodeWord(Index, _instructionSource);

        public bool Repeat { get; set; }

        public void UpdateBoard(IXyPair location) => Engine.UpdateBoard(location);

        public IWorld World => Engine.World;
    }
}