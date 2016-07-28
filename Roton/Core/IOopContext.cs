using System.Collections.Generic;
using Roton.Emulation.Mapping;

namespace Roton.Core
{
    public interface IOopContext : IExecutable, ISearchContext
    {
        IActor Actor { get; }
        ITile DeathTile { get; }
        IGrammar Grammar { get; }
        IList<string> Message { get; }
        int CommandsExecuted { get; set; }
        bool Died { get; set; }
        bool Executed { get; set; }
        bool Finished { get; set; }
        int Index { get; set; }
        bool Moved { get; set; }
        string Name { get; set; }
        bool NextLine { get; set; }
        int PreviousInstruction { get; set; }
        bool Repeat { get; set; }
        bool Resume { get; set; }
    }
}