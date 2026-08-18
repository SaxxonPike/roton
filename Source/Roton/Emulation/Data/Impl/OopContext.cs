using System;
using System.Collections.Generic;
using Roton.Emulation.Core;

namespace Roton.Emulation.Data.Impl;

public ref struct OopContext(IEngineAccessor engine)
{
    private readonly IEngine _engine = engine.Instance;

    public IActor Actor => _engine.Actors[Index];

    public int CommandsExecuted;

    public Tile DeathTile;

    public bool Died;

    public bool Executed;

    public bool Finished;

    public int Index;

    public List<string> Message { get; set; }

    public bool Moved;

    public string Name;

    public bool NextLine;

    public int PreviousInstruction;

    public bool Repeat;

    public bool Resume;

    public SearchContext Search;

    public int Command;

    public void AddMessage(ReadOnlySpan<char> message)
    {
        Message ??= [];
        Message.Add(message.ToString());
    }

    public bool HasMessage => Message != null;
}