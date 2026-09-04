using System;

namespace Roton.Emulation.Data;

public interface IElement
{
    /// <remarks>
    /// RoZ: ParamBoardName
    /// </remarks>
    string BoardEditText { get; set; }

    /// <remarks>
    /// RoZ: Character
    /// </remarks>
    ref HWord Character { get; }

    /// <remarks>
    /// RoZ: ParamTextName
    /// </remarks>
    string CodeEditText { get; set; }

    /// <remarks>
    /// RoZ: Color
    /// </remarks>
    ref HWord Color { get; }

    /// <remarks>
    /// RoZ: Cycle
    /// </remarks>
    ref Word Cycle { get; }

    /// <remarks>
    /// RoZ: CategoryName
    /// </remarks>
    string EditorCategory { get; set; }

    /// <summary>
    /// RoZ: HasDrawProc
    /// </summary>
    ref Bool HasDrawCode { get; }

    int Id { get; }

    /// <remarks>
    /// RoZ: VisibleInDark
    /// </remarks>
    ref Bool IsAlwaysVisible { get; }

    /// <remarks>
    /// RoZ: Destructible
    /// </remarks>
    ref Bool IsDestructible { get; }

    /// <remarks>
    /// RoZ: PlaceableOnTop
    /// </remarks>
    ref Bool IsEditorFloor { get; }

    /// <remarks>
    /// RoZ: Walkable
    /// </remarks>
    ref Bool IsFloor { get; }

    /// <remarks>
    /// RoZ: Pushable
    /// </remarks>
    ref Bool IsPushable { get; }

    /// <remarks>
    /// RoZ: EditorCategory
    /// </remarks>
    ref Word MenuIndex { get; }

    /// <remarks>
    /// RoZ: EditorShortcut
    /// </remarks>
    ref PChar MenuKey { get; }

    /// <remarks>
    /// RoZ: Name
    /// </remarks>
    string Name { get; set; }

    /// <remarks>
    /// RoZ: Param1Name
    /// </remarks>
    string P1EditText { get; set; }

    /// <remarks>
    /// RoZ: Param2Name
    /// </remarks>
    string P2EditText { get; set; }

    /// <remarks>
    /// RoZ: Param3Name
    /// </remarks>
    string P3EditText { get; set; }

    /// <remarks>
    /// RoZ: ScoreValue
    /// </remarks>
    ref Word Points { get; }

    /// <remarks>
    /// RoZ: ParamDirName
    /// </remarks>
    string StepEditText { get; set; }

    bool CanContainCode { get; }

    bool NameMatches(ReadOnlySpan<char> name);
}