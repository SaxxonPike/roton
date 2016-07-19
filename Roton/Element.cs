using System;

namespace Roton
{
    public partial class Element
    {
        internal virtual Action<int> Act { get; set; }
        public virtual string Board { get; internal set; }
        public virtual string Category { get; internal set; }
        public virtual int Character { get; internal set; }
        public virtual string Code { get; internal set; }
        public virtual int Color { get; internal set; }
        public virtual int Cycle { get; internal set; }
        public virtual bool Destructible { get; internal set; }
        internal virtual Func<IXyPair, AnsiChar> Draw { get; set; }
        public virtual bool DrawCodeEnable { get; internal set; }
        public virtual bool EditorFloor { get; internal set; }
        public virtual bool Floor { get; internal set; }
        public virtual int Index { get; internal set; }
        internal virtual Action<IXyPair, int, IXyPair> Interact { get; set; }
        public virtual int Key { get; internal set; }
        internal virtual string KnownName { get; set; }
        public virtual int Menu { get; internal set; }
        public virtual string Name { get; internal set; }
        public virtual string P1 { get; internal set; }
        public virtual string P2 { get; internal set; }
        public virtual string P3 { get; internal set; }
        public virtual int Points { get; internal set; }
        public virtual bool Pushable { get; internal set; }
        public virtual bool Shown { get; set; }
        public virtual string Step { get; internal set; }
    }
}