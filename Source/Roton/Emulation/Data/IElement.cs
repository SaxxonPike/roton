using System;

namespace Roton.Emulation.Data;

public interface IElement
{
    string BoardEditText { get; set; }
    ref HWord Character { get; }
    string CodeEditText { get; set; }
    ref HWord Color { get; }
    ref Word Cycle { get; }
    string EditorCategory { get; set; }
    ref Bool HasDrawCode { get; }
    int Id { get; }
    ref Bool IsAlwaysVisible { get; }
    ref Bool IsDestructible { get; }
    ref Bool IsEditorFloor { get; }
    ref Bool IsFloor { get; }
    ref Bool IsPushable { get; }
    ref Word MenuIndex { get; }
    ref PChar MenuKey { get; }
    string Name { get; set; }
    string P1EditText { get; set; }
    string P2EditText { get; set; }
    string P3EditText { get; set; }
    ref Word Points { get; }
    string StepEditText { get; set; }

    bool CanContainCode { get; }
    bool NameMatches(ReadOnlySpan<char> name);
}