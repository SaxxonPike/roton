using System;

namespace Roton.Core
{
    public interface IElement
    {
        Action<int> Act { get; set; }
        string BoardEditText { get; set; }
        int Character { get; set; }
        string CodeEditText { get; set; }
        int Color { get; set; }
        int Cycle { get; set; }
        Func<IXyPair, AnsiChar> Draw { get; set; }
        string EditorCategory { get; set; }
        bool HasDrawCode { get; set; }
        int Id { get; set; }
        Action<IXyPair, int, IXyPair> Interact { get; set; }
        bool IsAlwaysVisible { get; set; }
        bool IsDestructible { get; set; }
        bool IsEditorFloor { get; set; }
        bool IsFloor { get; set; }
        bool IsPushable { get; set; }
        string KnownName { get; set; }
        int MenuIndex { get; set; }
        int MenuKey { get; set; }
        string Name { get; set; }
        string P1EditText { get; set; }
        string P2EditText { get; set; }
        string P3EditText { get; set; }
        int Points { get; set; }
        string StepEditText { get; set; }
    }
}