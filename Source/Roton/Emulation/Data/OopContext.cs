using System;
using System.Collections.Generic;

namespace Roton.Emulation.Data;

public ref struct OopContext(IActorList actors)
{
    private List<string>? _message;

    public IActor Actor => actors[Index];

    public int CommandsExecuted;

    public Tile DeathTile;

    public bool Died;

    public bool Executed;

    public bool Finished;

    public int Index;

    public bool Moved;

    public string? Name;

    public bool NextLine;

    public int PreviousInstruction;

    public bool Repeat;

    public bool Resume;

    public SearchContext Search;

    public char Command;

    public void AddMessage(ReadOnlySpan<char> message)
    {
        _message ??= [];
        _message.Add(message.ToString());
    }

    public bool HasMessage => _message != null;

    public IReadOnlyList<string> GetMessage() => _message ?? [];
}