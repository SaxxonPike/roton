using System.Collections.Generic;
using Roton.Emulation.Core;

namespace Roton.Emulation.Data.Impl;

public sealed class OopContext : IOopContext
{
    private readonly IEngine _engine;

    internal OopContext(
        IEngineAccessor engine)
    {
        _engine = engine.Instance;
    }

    public int Instruction
    {
        get => InstructionSource.Instruction;
        set => InstructionSource.Instruction = value;
    }

    public IActor Actor => _engine.Actors[Index];

    public int CommandsExecuted { get; set; }

    public Tile DeathTile { get; set; }

    public bool Died { get; set; }

    public bool Executed { get; set; }

    public bool Finished { get; set; }

    public IExecutable InstructionSource { get; set; }

    public int Index { get; set; }

    public bool HasMessage => Message.Count > 0;

    public IList<string> Message { get; } = new List<string>();

    public bool Moved { get; set; }

    public string Name { get; set; }

    public bool NextLine { get; set; }

    public int PreviousInstruction { get; set; }

    public bool Repeat { get; set; }

    public bool Resume { get; set; }

    public int SearchIndex { get; set; }

    public int SearchOffset { get; set; }

    public int Command { get; set; }
}