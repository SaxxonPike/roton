using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.Execution
{
    internal class OopContext : IOopContext
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

        public IEngine Engine { get; }

        public int Instruction
        {
            get { return _instructionSource.Instruction; }
            set { _instructionSource.Instruction = value; }
        }

        public IActor Actor => Engine.Actors[_index];

        public int CommandsExecuted { get; set; }

        public ITile DeathTile { get; }

        public bool Died { get; set; }

        public bool Finished { get; set; }

        public IGrammar Grammar => Engine.Grammar;

        public int Index { get; set; }

        public string Message { get; set; }

        public bool Moved { get; set; }

        public string Name { get; set; }

        public bool NextLine { get; set; }

        public int PreviousInstruction { get; set; }

        public bool Repeat { get; set; }

        public int SearchIndex { get; set; }

        public int SearchOffset { get; set; }

        public string SearchTarget { get; set; }
    }
}