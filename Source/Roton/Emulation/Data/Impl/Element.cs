using System;

namespace Roton.Emulation.Data.Impl;

public abstract class Element(int id) : IElement
{
    public abstract string BoardEditText { get; set; }
    public abstract ref HWord Character { get; }
    public abstract string CodeEditText { get; set; }
    public abstract ref HWord Color { get; }
    public abstract ref Word Cycle { get;  }
    public abstract string EditorCategory { get; set; }
    public abstract ref Bool HasDrawCode { get; }
    public int Id { get; } = id;
    public abstract ref Bool IsAlwaysVisible { get; }
    public abstract ref Bool IsDestructible { get; }
    public abstract ref Bool IsEditorFloor { get; }
    public abstract ref Bool IsFloor { get; }
    public abstract ref Bool IsPushable { get; }
    public abstract ref Word MenuIndex { get; }
    public abstract ref PChar MenuKey { get; }
    public abstract string Name { get; set; }
    public abstract string P1EditText { get; set; }
    public abstract string P2EditText { get; set; }
    public abstract string P3EditText { get; set; }
    public abstract ref Word Points { get; }
    public abstract string StepEditText { get; set; }

    public abstract bool CanContainCode { get; }
    public abstract bool NameMatches(ReadOnlySpan<char> name);
}