using System;

namespace Roton.Emulation.Data;

public ref struct OopContext
{
    /// <summary>
    /// The actor that is executing the current command.
    /// </summary>
    public IActor Actor;

    /// <summary>
    /// Number of commands that have been executed.
    /// </summary>
    public int CommandsExecuted;

    /// <summary>
    /// If true, execution will repeat after message content is shown.
    /// </summary>
    public bool Continue;

    /// <summary>
    /// Tile data for replacing the actor once code has completed.
    /// </summary>
    public Tile DeathTile;

    /// <summary>
    /// If true, <see cref="DeathTile"/> will be plotted on the actor prior to removal.
    /// </summary>
    public bool Died;

    /// <summary>
    /// If true, <see cref="CommandsExecuted"/> will be incremented for the current command.
    /// </summary>
    public bool Executed;

    /// <summary>
    /// If true, whether by the #END command or reaching the end of code, execution has finished.
    /// </summary>
    public bool Finished;

    /// <summary>
    /// Index of the actor being processed.
    /// </summary>
    public int Index;

    /// <summary>
    /// If true, the actor has moved. A successful move suspends further execution until the next tick.
    /// </summary>
    public bool Moved;

    /// <summary>
    /// Name that will be shown on the top of message content.
    /// </summary>
    public ReadOnlySpan<char> Name;

    /// <summary>
    /// If true, bytes shall be skipped until the start of a new line.
    /// </summary>
    public bool NextLine;

    /// <summary>
    /// The instruction pointer when execution of the current command started.
    /// </summary>
    public int PreviousInstruction;

    /// <summary>
    /// If true, the current command shall be repeated.
    /// </summary>
    public bool Repeat;

    /// <summary>
    /// If true, execution shall continue on the same line.
    /// </summary>
    public bool Resume;

    /// <summary>
    /// Search context for manipulating labels.
    /// </summary>
    public SearchContext Search;

    /// <summary>
    /// Command byte of the current line.
    /// </summary>
    public char Command;
}