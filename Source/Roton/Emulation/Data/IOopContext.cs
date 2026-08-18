using System.Collections.Generic;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data;

public interface IOopContext : IExecutable, ISearchContext
{
    IActor Actor { get; }
    Tile DeathTile { get; set; }
    IList<string> Message { get; }
    int CommandsExecuted { get; set; }
    bool Died { get; set; }
    bool Executed { get; set; }
    bool Finished { get; set; }
    IExecutable InstructionSource { get; set; }
    bool Moved { get; set; }
    string Name { get; set; }
    bool NextLine { get; set; }
    int PreviousInstruction { get; set; }
    bool Repeat { get; set; }
    bool Resume { get; set; }
    int Command { get; set; }
    int Index { get; set; }
    bool HasMessage { get; }
}
