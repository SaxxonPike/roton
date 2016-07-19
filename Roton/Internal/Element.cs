using System;
using Roton.Core;

namespace Roton.Internal
{
    internal partial class Element : IElement
    {
        public virtual Action<int> Act { get; set; }
        public virtual string Board { get; set; }
        public virtual string Category { get; set; }
        public virtual int Character { get; set; }
        public virtual string Code { get; set; }
        public virtual int Color { get; set; }
        public virtual int Cycle { get; set; }
        public virtual bool Destructible { get; set; }
        public virtual Func<IXyPair, AnsiChar> Draw { get; set; }
        public virtual bool DrawCodeEnable { get; set; }
        public virtual bool EditorFloor { get; set; }
        public virtual bool Floor { get; set; }
        public virtual int Index { get; set; }
        public virtual Action<IXyPair, int, IXyPair> Interact { get; set; }
        public virtual int Key { get; set; }
        public virtual string KnownName { get; set; }
        public virtual int Menu { get; set; }
        public virtual string Name { get; set; }
        public virtual string P1 { get; set; }
        public virtual string P2 { get; set; }
        public virtual string P3 { get; set; }
        public virtual int Points { get; set; }
        public virtual bool Pushable { get; set; }
        public virtual bool Shown { get; set; }
        public virtual string Step { get; set; }
    }
}