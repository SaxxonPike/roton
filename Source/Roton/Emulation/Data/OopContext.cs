using System;

namespace Roton.Emulation.Data;

public ref struct OopContext
{
    public IActor Actor;

    public int CommandsExecuted;

    public Tile DeathTile;

    public bool Died;

    public bool Executed;

    public bool Finished;

    public int Index;

    public bool Moved;

    public ReadOnlySpan<char> Name;

    public bool NextLine;

    public int PreviousInstruction;

    public bool Repeat;

    public bool Resume;

    public SearchContext Search;

    public char Command;
}