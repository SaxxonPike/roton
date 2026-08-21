using System;
using System.Collections.Generic;
using Roton.Emulation.Core;

namespace Roton.Emulation.Data;

public ref struct OopContext(IEngineAccessor engine)
{
    private List<string>? _message;
    private readonly IEngine _engine = engine.Instance;

    public IActor Actor => _engine.Actors[Index];

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