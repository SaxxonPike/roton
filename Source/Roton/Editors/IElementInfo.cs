using System.Collections.Generic;

namespace Roton.Editors;

public enum ElementParameter
{
    P1 = 1,
    P2,
    BulletType,
    Board,
    Vector,
    Code,
}

public interface IElementInfo
{
    string? FriendlyName { get; }
    int Character { get; }
    int Color { get; }
    bool IsDestructible { get; }
    bool IsPushable { get; }
    bool IsPlaceableOnTop { get; }
    bool IsWalkable { get; }
    bool HasDrawProc { get; }
    int Cycle { get; }
    int Category { get; }
    char Shortcut { get; }
    string? Name { get; }
    string? CategoryName { get; }
    IReadOnlyDictionary<int, string> ParameterNames { get; }
    
}