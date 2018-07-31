using System.Collections.Generic;
using Roton.Emulation.Core;

namespace Roton.Emulation.Data.Impl
{
    public sealed class OopContext : IOopContext
    {
        private readonly int _index;
        
        private readonly IExecutable _instructionSource;
        private readonly IEngine _engine;

        public OopContext(
            int index,
            IExecutable instructionSource,
            string name,
            IEngine engine)
        {
            _instructionSource = instructionSource;
            _engine = engine;
            _index = index;
            Index = index;
            Name = name;
            DeathTile = new Tile(0, 0);
            Message = new List<string>();
        }

        public int Instruction
        {
            get => _instructionSource.Instruction;
            set => _instructionSource.Instruction = value;
        }

        public IActor Actor => _engine.Actors[_index];

        public int CommandsExecuted { get; set; }

        public ITile DeathTile { get; }

        public bool Died { get; set; }

        public bool Executed { get; set; }

        public bool Finished { get; set; }

        public int Index { get; set; }

        public IList<string> Message { get; }

        public bool Moved { get; set; }

        public string Name { get; set; }

        public bool NextLine { get; set; }

        public int PreviousInstruction { get; set; }

        public bool Repeat { get; set; }

        public bool Resume { get; set; }

        public int SearchIndex { get; set; }

        public int SearchOffset { get; set; }

        public string SearchTarget { get; set; }
        
        public int Command { get; set; }
    }
}