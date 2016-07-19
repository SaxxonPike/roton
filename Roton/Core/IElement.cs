using System;

namespace Roton.Core
{
    public interface IElement
    {
        Action<int> Act { get; set; }
        string Board { get; set; }
        string Category { get; set; }
        int Character { get; set; }
        string Code { get; set; }
        int Color { get; set; }
        int Cycle { get; set; }
        bool Destructible { get; set; }
        Func<IXyPair, AnsiChar> Draw { get; set; }
        bool DrawCodeEnable { get; set; }
        bool EditorFloor { get; set; }
        bool Floor { get; set; }
        int Index { get; set; }
        Action<IXyPair, int, IXyPair> Interact { get; set; }
        int Key { get; set; }
        string KnownName { get; set; }
        int Menu { get; set; }
        string Name { get; set; }
        string P1 { get; set; }
        string P2 { get; set; }
        string P3 { get; set; }
        int Points { get; set; }
        bool Pushable { get; set; }
        bool Shown { get; set; }
        string Step { get; set; }
    }
}
