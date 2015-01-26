using Roton.Emulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public partial class Element
    {
        virtual internal Action<int> Act { get; set; }
        virtual public string Board { get; internal set; }
        virtual public string Category { get; internal set; }
        virtual public int Character { get; internal set; }
        virtual public string Code { get; internal set; }
        virtual public int Color { get; internal set; }
        virtual public int Cycle { get; internal set; }
        virtual public bool Destructible { get; internal set; }
        virtual internal Func<Location, AnsiChar> Draw { get; set; }
        virtual public bool DrawCodeEnable { get; internal set; }
        virtual public bool EditorFloor { get; internal set; }
        virtual public bool Floor { get; internal set; }
        virtual public int Index { get; internal set; }
        virtual internal Action<Location, int, Vector> Interact { get; set; }
        virtual public int Key { get; internal set; }
        virtual internal string KnownName { get; set; }
        virtual public int Menu { get; internal set; }
        virtual public string Name { get; internal set; }
        virtual public string P1 { get; internal set; }
        virtual public string P2 { get; internal set; }
        virtual public string P3 { get; internal set; }
        virtual public int Points { get; internal set; }
        virtual public bool Pushable { get; internal set; }
        virtual public bool Shown { get; set; }
        virtual public string Step { get; internal set; }
    }
}
